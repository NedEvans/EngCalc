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
    └── Global Constants
```

### Core Entities

- **Project:** Top-level container (e.g., "Fimiston Gold")
- **Job:** Work area within project (e.g., "Milling Area") - contains GlobalConstants
- **Calculation:** Engineering analysis (e.g., "Bolted Moment Connection") - has multiple Revisions
- **CalculationRevision:** Versioned snapshot of calculation state
- **Card:** Individual engineering check template (e.g., "Tension Check", "Moment Check")
- **CardInstance:** Actual card with values in a calculation
- **AppConstant:** Application-level constant definitions (e.g., AS4100 steel properties) - template for GlobalConstants
- **GlobalConstant:** Job-level constants copied from AppConstants, can be customized per job
- **MaterialLibrary:** Pre-defined material properties organized by standard (AS4100, AS3600)

## Card System (Core Feature)

Cards are the fundamental calculation unit. Each card contains:
- **CodeTemplate:** C# code that performs calculations
- **MathMLFormula:** Visual representation of the calculation
- **InputVariables:** Parameters (JSON schema)
- **OutputVariables:** Results (JSON schema)
- **DesignLoad & CalculatedCapacity:** For pass/fail checks
- **CardVersion:** Template versioning for reusability

**Global Constants System:**

The system uses a two-tier constant architecture:

1. **AppConstants (Application Level):**
   - Seeded with AS4100/AS3600 standard values (e.g., fy_Grade300, Es_Steel, phi_Bending)
   - Users can create/edit AppConstants
   - Cannot be deleted if referenced by Card templates
   - Serve as templates for job-level constants

2. **GlobalConstants (Job Level):**
   - Created when Job is initialized - all AppConstants are copied to the Job
   - Each job can customize constant values for project-specific requirements
   - Can add custom constants specific to a job
   - Referenced by CardInstances during calculations

**Binding Flow:**

1. **Template Design (Card):**
   - Designer binds input variables to AppConstants by ID
   - Dropdown shows "ConstantName - Description" (never shows IDs to user)
   - Stored in Card.InputVariables JSON: `{ "name": "fy", "appConstantId": 5 }`

2. **Instance Creation (CardInstance in Calculation):**
   - System maps AppConstantId → GlobalConstantId (job's copy of that constant)
   - Stores in CardInstance.GlobalConstantBindings: `{ "fy": 123 }` (GlobalConstantId)
   - Default: "Use Global" checkbox is checked, showing constant value

3. **Instance Editing:**
   - User sees bound constant with value (read-only)
   - Can uncheck "Use Global" to override with manual value
   - Override state stored in CardInstance.GlobalConstantOverrides: `{ "fy": true }`

4. **Calculation Execution:**
   - Resolve GlobalConstants to actual values
   - Merge with overrides (local values take precedence)
   - Execute calculation
   - **Snapshot input values** in CardInstance.InputSnapshot for audit trail

**Missing Constant Handling:**
- If GlobalConstant doesn't exist in old job: checkbox unchecked, manual entry required
- Option to "Add this constant to job" (creates GlobalConstant from AppConstant)

**Unit Validation:**
- Display warning if input variable unit ≠ bound constant unit
- Does not block binding (user may have valid reason)

**Calculation Revisions:**
- Revisions are created **manually only** (user clicks "Create Revision")
- Each revision is a complete snapshot of the calculation at that point in time
- On revision creation, user chooses:
  - **Clone existing cards:** Start from current state
  - **Start empty:** Add cards fresh
- CardInstances are cloned with all bindings and values
- Input values used in calculations are snapshotted for audit trail
- Old revisions remain immutable even if GlobalConstant values change

**Calculation Flow:**
1. User enters/overrides input values
2. System resolves GlobalConstants and merges with local overrides
3. System snapshots input values to CardInstance.InputSnapshot
4. System executes C# code with merged variables
5. System calculates results and capacity
6. System compares design load to capacity
7. Status determined: Pass (Green), Warning (Amber), Fail (Red)
8. Results stored with input snapshot for audit compliance

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
│   │   ├── AppConstant.cs
│   │   ├── GlobalConstant.cs
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
2. **Revision Control:** All calculations maintain history via CalculationRevision entities (manual snapshots)
3. **Template Reusability:** Card templates are versioned and reusable across calculations
4. **Two-Tier Constants:** AppConstants (application-wide standards) → GlobalConstants (job-specific values)
5. **Audit Compliance:** Input values are snapshotted with calculation results for historical accuracy
6. **AI-Assisted but Human-Reviewed:** All AI-generated code must be reviewed before use
7. **MathML Standard:** Use MathML v3 for formula display (accessible, printable, no dependencies)
8. **ID-Based Binding:** Use database IDs (not string matching) to bind variables to constants

## Implementation Phases

The PRD outlines a 10-phase implementation:
1. **Foundation:** Infrastructure, data model, EF Core, basic CRUD ✅ **COMPLETE**
2. **Calculation & Card Structure:** Revisions, templates, variable injection ✅ **MOSTLY COMPLETE** (Revision UI pending)
3. **AI Integration:** Card generation from engineering texts (moved early for productivity)
4. **Calculation Engine:** C# execution, result storage, pass/fail logic ✅ **COMPLETE**
5. **MathML Rendering:** Formula display with variable substitution
6. **Material Library:** AS4100/AS3600 standards, reusable properties
7. **AI Enhancement:** Card updates, bulk generation, validation
8. **PDF Export:** Calculation reports with formulas and branding
9. **Authentication:** User roles (Admin, Engineer, Viewer), audit logging
10. **Production Hardening:** Backups, reverse proxy, performance optimization

### Phase 2 Enhancement: Global Constants System

**Objective:** Implement two-tier constant architecture with AppConstants and enhanced GlobalConstants

**Implementation Plan:**

**Stage 1: Data Model & Database (Foundation)**
1. Create `AppConstant` model with properties:
   - AppConstantId, ConstantName, DefaultValue, Unit, Standard, Description, Category
2. Update `GlobalConstant` model:
   - Add `AppConstantId` (nullable FK) to track origin
   - Keep existing: GlobalConstantId, JobId, ConstantName, ConstantValue, Unit, Description
3. Update `CardInstance` model:
   - Add `GlobalConstantBindings` (JSON): Maps variable name → GlobalConstantId
   - Add `GlobalConstantOverrides` (JSON): Tracks which variables use local vs global
   - Add `InputSnapshot` (JSON): Stores actual input values used in calculation
4. Create EF migration for new table and columns
5. Seed `AppConstants` table with AS4100/AS3600 standard values:
   - Steel: fy_Grade250/300/350/400/450, Es_Steel (200000 MPa), nu_Steel (0.3)
   - Concrete: fc_20/25/32/40/50/65/80/100, Ec values
   - Factors: phi_Bending (0.9), phi_Compression (0.6), phi_Shear (0.7), phi_Tension (0.9)

**Stage 2: Backend Logic**
6. Update Job creation logic (`Jobs.razor` SaveJob method):
   - On new Job: Copy all AppConstants → GlobalConstants for that Job
   - Set GlobalConstant.AppConstantId to track origin
7. AppConstant CRUD with deletion protection:
   - New page `/app-constants` for managing AppConstants
   - Check Card references before deletion
   - Show warning with list of affected templates
8. CardInstance creation logic:
   - Read Card.InputVariables to find appConstantId bindings
   - Map AppConstantId → GlobalConstantId (find job's copy)
   - Initialize GlobalConstantBindings with mapped IDs
   - Set GlobalConstantOverrides to false (use global by default)
   - Handle missing GlobalConstants gracefully
9. Calculation execution:
   - Resolve GlobalConstantIds to actual values
   - Merge with local overrides (local takes precedence)
   - Create InputSnapshot JSON with actual values used
   - Store snapshot before execution for audit trail

**Stage 3: UI - Template Editor**
10. Update `CardTemplateDetail.razor`:
    - Modify InputVariables table to include "Use Global Constant" column
    - Add checkbox per variable
    - When checked, show dropdown of AppConstants
    - Dropdown displays: "ConstantName - Description (DefaultValue Unit)"
    - Filter dropdown by matching unit (with warning if mismatch)
    - Store appConstantId in InputVariables JSON schema
11. Load all AppConstants for dropdown population
12. Visual indicators for bound variables

**Stage 4: UI - Instance Editor (Major Rewrite)**
13. Rewrite `CardInstanceEditor.razor` input handling:
    - For each input variable, check if Card has appConstantId binding
    - If bound:
      - Resolve AppConstantId → GlobalConstantId for current job
      - Display: Variable name, bound constant info (name, value, unit)
      - Show "Use Global" checkbox (checked by default)
      - When checked: Show value read-only
      - When unchecked: Enable textbox for manual override
    - If not bound: Show normal textbox
    - Track override state in component state
14. Handle missing GlobalConstants:
    - Show warning: "Constant not available in this job"
    - Default to unchecked + manual entry
    - Add "+ Add to Job" button → opens modal
15. Add New Constant modal:
    - Modal form: ConstantName, Value, Unit, Description
    - Option to "Copy from AppConstant" (loads defaults)
    - On save: Create GlobalConstant for current job
    - Refresh dropdown/bindings
    - Close modal and auto-select new constant
16. Unit mismatch warnings (amber alert)
17. Visual design: Compact layout, clear indication of global vs local

**Stage 5: UI - AppConstant Management**
18. Create `/app-constants` page:
    - Table with columns: Name, Default Value, Unit, Standard, Description
    - CRUD operations: Create, Edit, Delete (with protection)
    - Group by Standard/Category in display
    - Search/filter functionality
19. Deletion protection dialog:
    - Query Cards using this AppConstantId
    - Show list of affected templates
    - Block deletion with clear message
20. Navigation: Add link in main menu or settings

**Stage 6: Revision Management UI**
21. Add "Create Revision" button to CalculationDetail page
22. Modal on revision creation:
    - Option 1: Clone existing cards (radio button)
    - Option 2: Start empty (radio button)
    - Revision number auto-increment
    - Comments field
23. Clone logic:
    - Deep copy all CardInstances
    - Copy all bindings, overrides, values
    - Link to new CalculationRevision
24. Display revision history and allow switching between revisions

**Stage 7: Testing & Bug Fixes**
25. Fix back button visibility issue (investigate CSS/rendering)
26. Test complete flow: AppConstant → GlobalConstant → Binding → Calculation → Snapshot
27. Test edge cases: Missing constants, old jobs, unit mismatches
28. Test revision cloning and immutability
29. Verify audit trail (input snapshot accuracy)

**Stage 8: Documentation & Cleanup**
30. Update user documentation
31. Add inline help tooltips
32. Code cleanup and refactoring
33. Performance optimization (minimize DB queries)

**Implementation Status:**

✅ **COMPLETED (Stages 1-5):**
- Stage 1: Data Model & Database - AppConstant, GlobalConstant, CardInstance models with migrations
- Stage 2: Backend Logic - Job creation, CardInstance binding, calculation snapshots
- Stage 3: Template Editor UI - CardTemplates.razor visual editor with binding UI, AppConstant dropdown, unit filtering, unit mismatch warnings
- Stage 4: Instance Editor UI - CardInstanceEditor with "Use Global" checkboxes and override functionality
- Stage 5: AppConstant Management - `/app-constants` page with CRUD operations, search/filter

❌ **PENDING (Stages 6-8):**
- Stage 6: Revision Management UI - Create revision button, clone/empty options, revision switching
- Stage 7: Testing & Bug Fixes - Comprehensive testing of global constants flow
- Stage 8: Documentation & Cleanup - User docs, tooltips, performance optimization

**Seeded Card Templates with Bindings:**
- **Plate Bending Capacity**: `yieldStrength` → `fy_Grade300` (AppConstantId lookup)
- **Bolt Tension Check**: `tensileStrength` → `fu_Grade430` (AppConstantId lookup)
- **Simply Supported Beam**: No bindings (uses local values only)

**Testing Instructions:**
1. Delete all test data from backend (Cards, Projects, Jobs, etc.)
2. Restart application to trigger reseeding
3. Navigate to "App Constants" to view 48 seeded constants
4. Create new Job → auto-copies all AppConstants to GlobalConstants
5. Create Calculation → Add card → View bindings in CardInstanceEditor
6. Test "Use Global Constant" checkbox and override functionality

## Reference Materials

- `engineering_calc_prd.md` - Complete product requirements document
- `mathmlv3.pdf` - MathML specification for formula rendering
- `SCI_P938.pdf` - Example engineering text for AI training/testing

## Engineering Standards

- AS4100: Australian Steel Structures Standard
- AS3600: Australian Concrete Structures Standard
- SCI Publications: Steel Construction Institute references
