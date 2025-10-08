using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

namespace EngineeringCalc.Services;

public class CalculationEngine
{
    public async Task<CalculationResult> ExecuteCalculation(
        string codeTemplate,
        Dictionary<string, object> inputVariables)
    {
        try
        {
            // Compile the code
            var assembly = await CompileCode(codeTemplate);
            if (assembly == null)
            {
                return new CalculationResult
                {
                    Success = false,
                    ErrorMessage = "Failed to compile calculation code"
                };
            }

            // Find and execute the Calculate method
            var result = await ExecuteCalculateMethod(assembly, inputVariables);
            return result;
        }
        catch (Exception ex)
        {
            return new CalculationResult
            {
                Success = false,
                ErrorMessage = $"Execution error: {ex.Message}"
            };
        }
    }

    private async Task<Assembly?> CompileCode(string code)
    {
        // Wrap the code in a class if it's just a method
        var fullCode = WrapCodeInClass(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);

        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Math).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
        };

        // Use unique assembly name to avoid conflicts
        var assemblyName = $"DynamicCalculation_{Guid.NewGuid():N}";

        var compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        EmitResult result = compilation.Emit(ms);

        if (!result.Success)
        {
            var failures = result.Diagnostics.Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error);
            var errorMessages = string.Join("\n", failures.Select(f => f.GetMessage()));
            throw new InvalidOperationException($"Compilation failed:\n{errorMessages}");
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
        return await Task.FromResult(assembly);
    }

    private string WrapCodeInClass(string code)
    {
        // If code already contains a class definition, return as is
        if (code.Contains("class ") || code.Contains("public class"))
        {
            return code;
        }

        // Otherwise wrap in a simple class
        return $@"
using System;

public class CalculationClass
{{
    {code}
}}";
    }

    private async Task<CalculationResult> ExecuteCalculateMethod(Assembly assembly, Dictionary<string, object> inputs)
    {
        var result = new CalculationResult { Success = true };

        try
        {
            // Find the class and method
            var type = assembly.GetTypes().FirstOrDefault(t => t.IsClass && !t.IsAbstract);
            if (type == null)
            {
                result.Success = false;
                result.ErrorMessage = "No executable class found in compiled code";
                return result;
            }

            var method = type.GetMethod("Calculate");
            if (method == null)
            {
                result.Success = false;
                result.ErrorMessage = "Calculate method not found";
                return result;
            }

            var instance = Activator.CreateInstance(type);

            // Prepare parameters
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                if (param.IsOut)
                {
                    args[i] = Activator.CreateInstance(param.ParameterType.GetElementType()!);
                }
                else
                {
                    if (inputs.TryGetValue(param.Name!, out var value))
                    {
                        args[i] = Convert.ChangeType(value, param.ParameterType);
                    }
                    else
                    {
                        args[i] = Activator.CreateInstance(param.ParameterType)!;
                    }
                }
            }

            // Execute the method
            var returnValue = method.Invoke(instance, args);

            // Collect results
            result.ReturnValue = returnValue;
            result.OutputVariables = new Dictionary<string, object>();

            // Collect out parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].IsOut)
                {
                    result.OutputVariables[parameters[i].Name!] = args[i];
                }
            }

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Execution error: {ex.Message}";
            return result;
        }
    }
}

public class CalculationResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public object? ReturnValue { get; set; }
    public Dictionary<string, object> OutputVariables { get; set; } = new();
}
