using EngineeringCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringCalc.Data;

public static class SeedData
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        // Check if we already have data
        if (context.Cards.Any())
        {
            return; // Database has been seeded
        }

        // Look up AppConstant IDs for binding
        var fy_Grade300 = await context.AppConstants.FirstOrDefaultAsync(ac => ac.ConstantName == "fy_Grade300");
        var fu_Grade430 = await context.AppConstants.FirstOrDefaultAsync(ac => ac.ConstantName == "fu_Grade430");

        // Create sample card templates
        var boltTensionCard = new Card
        {
            CardName = "Bolt Tension Check",
            CardType = "Bolt Design",
            CardVersion = "1.0",
            Description = "Calculates bolt capacity in tension per AS4100",
            CodeTemplate = @"
public double Calculate(double boltDia, double tensileStrength, int numberOfBolts, out double capacity)
{
    // Calculate bolt area (mm²)
    double area = Math.PI * Math.Pow(boltDia / 2, 2);

    // Calculate total capacity (kN)
    capacity = area * tensileStrength * numberOfBolts / 1000.0;

    return capacity;
}",
            MathMLFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"">
  <mi>N</mi>
  <mo>=</mo>
  <mi>n</mi>
  <mo>×</mo>
  <mi>A</mi>
  <mo>×</mo>
  <msub><mi>f</mi><mi>u</mi></msub>
</math>",
            InputVariables = $@"[
  {{""name"": ""boltDia"", ""type"": ""double"", ""unit"": ""mm"", ""description"": ""Bolt diameter""}},
  {{""name"": ""tensileStrength"", ""type"": ""double"", ""unit"": ""MPa"", ""description"": ""Bolt tensile strength"", ""appConstantId"": {fu_Grade430?.AppConstantId}}},
  {{""name"": ""numberOfBolts"", ""type"": ""int"", ""unit"": """", ""description"": ""Number of bolts""}},
  {{""name"": ""designLoad"", ""type"": ""double"", ""unit"": ""kN"", ""description"": ""Design tension load""}}
]",
            OutputVariables = @"[
  {""name"": ""capacity"", ""type"": ""double"", ""unit"": ""kN"", ""description"": ""Bolt tension capacity""}
]",
            DesignLoadVariable = "designLoad",
            CapacityVariable = "capacity"
        };

        var beamBendingCard = new Card
        {
            CardName = "Simply Supported Beam Moment",
            CardType = "Beam Design",
            CardVersion = "1.0",
            Description = "Calculates maximum moment for simply supported beam with uniform load",
            CodeTemplate = @"
public double Calculate(double span, double uniformLoad, out double maxMoment)
{
    // M = wL²/8 for simply supported beam with uniform load
    maxMoment = (uniformLoad * Math.Pow(span, 2)) / 8.0;

    return maxMoment;
}",
            MathMLFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"">
  <mi>M</mi>
  <mo>=</mo>
  <mfrac>
    <mrow><mi>w</mi><msup><mi>L</mi><mn>2</mn></msup></mrow>
    <mn>8</mn>
  </mfrac>
</math>",
            InputVariables = @"[
  {""name"": ""span"", ""type"": ""double"", ""unit"": ""m"", ""description"": ""Beam span""},
  {""name"": ""uniformLoad"", ""type"": ""double"", ""unit"": ""kN/m"", ""description"": ""Uniform distributed load""},
  {""name"": ""designMoment"", ""type"": ""double"", ""unit"": ""kNm"", ""description"": ""Design moment capacity required""}
]",
            OutputVariables = @"[
  {""name"": ""maxMoment"", ""type"": ""double"", ""unit"": ""kNm"", ""description"": ""Maximum bending moment""}
]",
            DesignLoadVariable = "designMoment",
            CapacityVariable = "maxMoment"
        };

        var plateBendingCard = new Card
        {
            CardName = "Plate Bending Capacity",
            CardType = "Plate Design",
            CardVersion = "1.0",
            Description = "Calculates plate moment capacity per AS4100",
            CodeTemplate = @"
public double Calculate(double plateWidth, double plateThickness, double yieldStrength, out double momentCapacity)
{
    // Calculate section modulus Z = bt²/6 (mm³)
    double sectionModulus = (plateWidth * Math.Pow(plateThickness, 2)) / 6.0;

    // Moment capacity M = Z × fy (kNm)
    momentCapacity = sectionModulus * yieldStrength / 1000000.0; // Convert to kNm

    return momentCapacity;
}",
            MathMLFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"">
  <mi>M</mi>
  <mo>=</mo>
  <mi>Z</mi>
  <mo>×</mo>
  <msub><mi>f</mi><mi>y</mi></msub>
  <mspace width=""1em""/>
  <mo>,</mo>
  <mspace width=""1em""/>
  <mi>Z</mi>
  <mo>=</mo>
  <mfrac>
    <mrow><mi>b</mi><msup><mi>t</mi><mn>2</mn></msup></mrow>
    <mn>6</mn>
  </mfrac>
</math>",
            InputVariables = $@"[
  {{""name"": ""plateWidth"", ""type"": ""double"", ""unit"": ""mm"", ""description"": ""Plate width""}},
  {{""name"": ""plateThickness"", ""type"": ""double"", ""unit"": ""mm"", ""description"": ""Plate thickness""}},
  {{""name"": ""yieldStrength"", ""type"": ""double"", ""unit"": ""MPa"", ""description"": ""Steel yield strength"", ""appConstantId"": {fy_Grade300?.AppConstantId}}},
  {{""name"": ""designMoment"", ""type"": ""double"", ""unit"": ""kNm"", ""description"": ""Design moment""}}
]",
            OutputVariables = @"[
  {""name"": ""momentCapacity"", ""type"": ""double"", ""unit"": ""kNm"", ""description"": ""Moment capacity""}
]",
            DesignLoadVariable = "designMoment",
            CapacityVariable = "momentCapacity"
        };

        context.Cards.AddRange(boltTensionCard, beamBendingCard, plateBendingCard);
        context.SaveChanges();
    }
}
