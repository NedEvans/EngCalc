# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is an engineering calculations application built with **Blazor Server** and **C#**. The system organizes calculations hierarchically (Project → Job → Calculation → Card) and enables structural engineers to perform, document, and version-control engineering analyses.

**Key URLs:**
- Development: http://192.168.0.175:5227 (port 5227)
- Production: https://app.nedevans.au
- Database: SQL Server at 192.168.0.175,1433

## Technology Stack

- **Framework:** Blazor Server with server-side interactive rendering
- **Language:** C#
- **Database:** SQL Server with Entity Framework Core + Migrations
- **Math Rendering:** MathML v3 (see `mathmlv3.pdf`)
- **AI Integration:** Terminal-based (Codex/Claude CLI) for card generation

## Data Model Hierarchy

```
Project
├── Jobs (multiple)
    ├── Calculations (multiple)
        ├── Cards (multiple)
            ├── Variables (local)
            └── Results
    └── Global Variables
```

### Core Entities

- **Project:** Top-level container (e.g., "Fimiston Gold")
- **Job:** Work area within project (e.g., "Milling Area") - contains GlobalVariables
- **Calculation:** Engineering analysis (e.g., "Bolted Moment Connection") - has multiple Revisions
- **CalculationRevision:** Versioned snapshot of calculation state
- **Card:** Individual engineering check template (e.g., "Tension Check", "Moment Check")
- **CardInstance:** Actual card with values in a calculation
- **GlobalVariable:** Job-level variables (e.g., steel yield strength) inherited by all cards
- **MaterialLibrary:** Pre-defined material properties organized by standard (AS4100, AS3600)

## Card System (Core Feature)

Cards are the fundamental calculation unit. Each card contains:
- **CodeTemplate:** C# code that performs calculations
- **MathMLFormula:** Visual representation of the calculation
- **InputVariables:** Parameters (JSON schema)
- **OutputVariables:** Results (JSON schema)
- **DesignLoad & CalculatedCapacity:** For pass/fail checks
- **CardVersion:** Template versioning for reusability

**Variable Inheritance:**
- Global variables (defined at Job level) are automatically injected into all cards
- Local variables (card instance level) can override global variables
- Examples: Global = steel yield strength; Local = plate thickness, bolt diameter

**Calculation Flow:**
1. User exits input cell
2. System executes C# code with variable substitution
3. System calculates results and capacity
4. System compares design load to capacity
5. Status determined: Pass (Green), Warning (Amber), Fail (Red)

## AI-Assisted Card Generation

AI (via terminal CLI) reads engineering PDFs (like `SCI_P938.pdf`) and generates:
- C# calculation code
- MathML formula representation
- Input/output variable definitions
- Design load and capacity variable identification

**Workflow:**
1. Provide engineering text/standard reference
2. Specify card name and type
3. AI generates code and formula
4. Developer reviews, tests, modifies
5. Developer approves and saves template
6. AI can update existing cards (with review)

**Expected AI Output Format:**
```csharp
// Card: Bolt Tension Check
// Inputs: boltDiameter (mm), tensileStrength (MPa), numberOfBolts
// Outputs: capacity (kN)
// Design Load Variable: designTensionLoad
// Capacity Variable: capacity

public class BoltTensionCheck
{
    public double Calculate(double boltDiameter, double tensileStrength,
                          int numberOfBolts, out double capacity)
    {
        double area = Math.PI * Math.Pow(boltDiameter / 2, 2);
        capacity = area * tensileStrength * numberOfBolts / 1000; // kN
        return capacity;
    }
}

// MathML:
// <math><mi>N</mi><mo>=</mo><mi>n</mi><mo>×</mo><mi>A</mi><mo>×</mo><mi>f</mi></math>
```

## Project Structure

```
/home/coder/Calc/
├── EngineeringCalc/          # Main Blazor Server project
│   ├── Components/           # Blazor components
│   ├── Data/                # Database context
│   │   └── ApplicationDbContext.cs
│   ├── Models/              # Entity models
│   │   ├── Project.cs
│   │   ├── Job.cs
│   │   ├── Calculation.cs
│   │   ├── CalculationRevision.cs
│   │   ├── Card.cs
│   │   ├── CardInstance.cs
│   │   ├── GlobalVariable.cs
│   │   └── MaterialLibrary.cs
│   ├── Migrations/          # EF Core migrations
│   ├── .env                 # Environment variables (NOT in git)
│   └── Program.cs           # App entry point
├── .vscode/                 # VS Code configuration
│   ├── launch.json          # F5 debugging config
│   └── tasks.json           # Build tasks
├── CLAUDE.md               # This file
└── engineering_calc_prd.md # Full requirements

Database Name: EngineeringCalc
```

## Development Commands

**Running the Application:**
- Press F5 in VS Code (uses `.vscode/launch.json`)
- Or manually: `cd EngineeringCalc && dotnet run` (listens on http://0.0.0.0:5227)
- Access at: http://192.168.0.175:5227

**Database:**
- Add migration: `cd EngineeringCalc && dotnet ef migrations add <MigrationName>`
- Update database: `cd EngineeringCalc && dotnet ef database update`
- Remove last migration: `cd EngineeringCalc && dotnet ef migrations remove`

**Build & Test:**
- Build: `cd EngineeringCalc && dotnet build`
- Test: `cd EngineeringCalc && dotnet test`
- Watch (hot reload): Use VS Code task or `cd EngineeringCalc && dotnet watch`

**Environment Variables:**
Database credentials are stored in `EngineeringCalc/.env` (excluded from git):
- DB_SERVER=192.168.0.175,1433
- DB_NAME=EngineeringCalc
- DB_USER=sa
- DB_PASSWORD=Ch1ck4m4ug41*^3!

## Key Design Principles

1. **Server-Side Rendering:** Calculations execute server-side for security and performance
2. **Revision Control:** All calculations maintain history via CalculationRevision entities
3. **Template Reusability:** Card templates are versioned and reusable across calculations
4. **Variable Propagation:** Global variables flow down from Job → Cards automatically
5. **AI-Assisted but Human-Reviewed:** All AI-generated code must be reviewed before use
6. **MathML Standard:** Use MathML v3 for formula display (accessible, printable, no dependencies)

## Implementation Phases

The PRD outlines a 10-phase implementation:
1. **Foundation:** Infrastructure, data model, EF Core, basic CRUD
2. **Calculation & Card Structure:** Revisions, templates, variable injection
3. **AI Integration:** Card generation from engineering texts (moved early for productivity)
4. **Calculation Engine:** C# execution, result storage, pass/fail logic
5. **MathML Rendering:** Formula display with variable substitution
6. **Material Library:** AS4100/AS3600 standards, reusable properties
7. **AI Enhancement:** Card updates, bulk generation, validation
8. **PDF Export:** Calculation reports with formulas and branding
9. **Authentication:** User roles (Admin, Engineer, Viewer), audit logging
10. **Production Hardening:** Backups, reverse proxy, performance optimization

## Reference Materials

- `engineering_calc_prd.md` - Complete product requirements document
- `mathmlv3.pdf` - MathML specification for formula rendering
- `SCI_P938.pdf` - Example engineering text for AI training/testing

## Engineering Standards

- AS4100: Australian Steel Structures Standard
- AS3600: Australian Concrete Structures Standard
- SCI Publications: Steel Construction Institute references
