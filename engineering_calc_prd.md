# Product Requirements Document
## Engineering Calculations Application

**Version:** 1.0  
**Date:** October 6, 2025  
**Author:** Engineering Team

---

## 1. Executive Summary

### 1.1 Product Overview
A web-based engineering calculations application designed to streamline structural engineering checks through reusable calculation cards. The system organizes calculations hierarchically (Project → Job → Calculation → Card) and enables engineers to perform, document, and version-control their engineering analyses.

### 1.2 Goals
- Digitize and standardize engineering calculation workflows
- Enable reuse of calculation templates across projects
- Provide clear visual representation of mathematical formulas using MathML
- Maintain calculation history and revisions
- Accelerate card creation through AI-assisted code generation

### 1.3 Target Users
- Structural Engineers
- Engineering Managers
- Calculation Reviewers

---

## 2. Technical Architecture

### 2.1 Technology Stack
- **Framework:** Blazor Server with server-side interactive rendering
- **Language:** C#
- **Database:** SQL Server (192.168.0.175,1433)
- **ORM:** Entity Framework Core with Migrations
- **IDE:** VS Code Server (Docker container)
- **Math Rendering:** MathML v3
- **AI Integration:** Terminal-based (Codex/Claude CLI)

### 2.2 Infrastructure
- **Development URL:** http://192.168.0.175:5227 (exposed port 5227)
- **Production URL:** https://app.nedevans.au (reverse proxied)
- **Database Server:** SQL Server at 192.168.0.175,1433
- **Database Credentials:** Password=Ch1ck4m4ug41*^3!
- **Concurrent Users:** ~6 initially

### 2.3 Reference Documents
- `mathmlv3.pdf` - MathML specification for formula rendering
- `SCI_P938.pdf` - Example engineering text for AI training

---

## 3. Data Model

### 3.1 Entity Hierarchy
```
Project
├── Jobs (multiple)
    ├── Calculations (multiple)
        ├── Cards (multiple)
            ├── Variables (local)
            └── Results
    └── Global Variables
```

### 3.2 Core Entities

#### 3.2.1 Project
- ProjectId (PK)
- ProjectName (e.g., "Fimiston Gold")
- Description
- CreatedDate
- LastModifiedDate
- CreatedBy
- Status

#### 3.2.2 Job
- JobId (PK)
- ProjectId (FK)
- JobName (e.g., "Milling Area")
- Description
- CreatedDate
- LastModifiedDate
- GlobalVariables (JSON/separate table)

#### 3.2.3 Calculation
- CalculationId (PK)
- JobId (FK)
- CalculationTitle (e.g., "Bolted Moment Connection")
- Description
- CurrentRevision
- CreatedDate
- LastModifiedDate

#### 3.2.4 CalculationRevision
- RevisionId (PK)
- CalculationId (FK)
- RevisionNumber (e.g., "Rev1", "Rev2")
- CreatedDate
- CreatedBy
- Comments
- Status (Draft/Approved/Superseded)

#### 3.2.5 Card
- CardId (PK)
- CalculationRevisionId (FK)
- CardName
- CardType (e.g., "Tension Check", "Moment Check")
- CardVersion (for template versioning)
- DisplayOrder
- CodeTemplate (C# code)
- MathMLFormula
- InputVariables (JSON schema)
- OutputVariables (JSON schema)

#### 3.2.6 CardInstance
- CardInstanceId (PK)
- CardId (FK)
- CalculationRevisionId (FK)
- LocalVariables (JSON - actual values)
- CalculatedResults (JSON)
- DesignLoad
- CalculatedCapacity
- CheckResult (Pass/Fail/Warning)
- LastCalculated

#### 3.2.7 GlobalVariable
- GlobalVariableId (PK)
- JobId (FK)
- VariableName (e.g., "SteelYieldStrength")
- VariableValue
- Unit
- Description

#### 3.2.8 MaterialLibrary
- MaterialId (PK)
- MaterialType (Steel/Concrete/Timber)
- Grade (e.g., "Grade 300", "N40 Concrete")
- Properties (JSON - yield strength, ultimate strength, etc.)
- Standard (e.g., "AS4100", "AS3600")

---

## 4. Functional Requirements

### 4.1 Project Management (CRUD)

#### 4.1.1 Create Project
- User can create new project with name and description
- System generates unique ProjectId
- System timestamps creation

#### 4.1.2 Read Project
- User can view list of all projects
- User can view project details with associated jobs
- User can search/filter projects

#### 4.1.3 Update Project
- User can edit project name and description
- System tracks last modified date

#### 4.1.4 Delete Project
- User can delete project (with confirmation)
- System cascades delete to jobs, calculations, and cards OR prevents deletion if jobs exist

### 4.2 Job Management (CRUD)

#### 4.2.1 Create Job
- User can create new job within a project
- User can set global variables for the job
- User can initialize from material library

#### 4.2.2 Read Job
- User can view list of jobs within project
- User can view job details with calculations
- User can view/edit global variables

#### 4.2.3 Update Job
- User can edit job name, description
- User can modify global variables
- Changes to global variables propagate to cards

#### 4.2.4 Delete Job
- User can delete job with confirmation
- System handles cascade/prevention logic

### 4.3 Calculation Management (CRUD)

#### 4.3.1 Create Calculation
- User can create new calculation within a job
- User can specify calculation title and description
- System creates initial revision (Rev1)

#### 4.3.2 Read Calculation
- User can view list of calculations
- User can view calculation with all cards
- User can view calculation history/revisions

#### 4.3.3 Update Calculation
- User can edit calculation details
- User can create new revision
- User can revert to previous revision

#### 4.3.4 Delete Calculation
- User can delete calculation with confirmation

#### 4.3.5 Revision Management
- User can save current state as new revision
- User can add comments to revision
- User can compare revisions
- System maintains revision history

### 4.4 Card Management (Primary Focus)

#### 4.4.1 Card Template Creation
- User can create new card template
- User can define input variables (name, type, unit, default value)
- User can define output variables
- User can write/paste C# calculation code
- User can enter MathML formula for display
- User can specify design load variable
- User can specify capacity output variable
- User can save card template for reuse

#### 4.4.2 Card Instance Management
- User can add card to calculation from template library
- User can set local variables (e.g., plate thickness)
- User can view inherited global variables (e.g., steel yield strength)
- User can override global variables locally if needed
- User can set display order of cards within calculation

#### 4.4.3 Card Calculation Engine
- System evaluates card when user exits input cell
- System executes C# code with variable substitution
- System calculates results and capacity
- System compares design load to capacity
- System determines check status (Pass/Fail/Warning)
- System stores calculation timestamp

#### 4.4.4 Card Display
- System renders MathML formula with substituted values
- System displays input variables with units
- System displays calculated outputs with units
- System provides toggle for verbose mode (show intermediate steps)
- System color-codes results:
  - **Green:** Design Load < Capacity (Pass)
  - **Amber:** Design Load approaching Capacity (Warning)
  - **Red:** Design Load > Capacity (Fail)

#### 4.4.5 Card Versioning
- User can create new version of card template
- System maintains backward compatibility
- User can upgrade card instances to new version
- System tracks which version each instance uses

### 4.5 Variable Management

#### 4.5.1 Global Variables
- Defined at Job level
- Automatically injected into all cards in that job
- Examples: steel yield strength, concrete strength, load factors
- User can view/edit in job settings

#### 4.5.2 Local Variables
- Defined per card instance
- Examples: plate thickness, bolt diameter, length
- Override global variables if same name exists

#### 4.5.3 Material Library
- Pre-defined material properties
- User can select from library to populate global variables
- User can add custom materials
- Organized by type and standard (AS4100, AS3600, etc.)

### 4.6 AI-Assisted Card Generation

#### 4.6.1 AI Integration
- Terminal-based interface using Codex or Claude CLI
- AI reads engineering text (PDF format like SCI_P938.pdf)
- AI generates C# calculation code
- AI generates MathML formula representation
- AI identifies input and output variables
- AI identifies design load and capacity variables

#### 4.6.2 AI Workflow
1. User provides engineering text/standard reference
2. User specifies card name and type
3. AI generates code and formula
4. System presents generated code for review
5. User reviews, tests, and modifies as needed
6. User approves and saves card template
7. AI can update existing cards (subject to review)

#### 4.6.3 AI Output Format
```csharp
// Card: Bolt Tension Check
// Inputs: boltDiameter (mm), tensileStrength (MPa), numberOfBolts
// Outputs: capacity (kN)
// Design Load Variable: designTensionLoad
// Capacity Variable: capacity

public class BoltTensionCheck
{
    public double Calculate(double boltDiameter, double tensileStrength, int numberOfBolts, out double capacity)
    {
        double area = Math.PI * Math.Pow(boltDiameter / 2, 2);
        capacity = area * tensileStrength * numberOfBolts / 1000; // kN
        return capacity;
    }
}

// MathML:
// <math><mi>N</mi><mo>=</mo><mi>n</mi><mo>×</mo><mi>A</mi><mo>×</mo><mi>f</mi></math>
```

---

## 5. Non-Functional Requirements

### 5.1 Performance
- Page load time < 2 seconds
- Calculation execution < 500ms per card
- Support 6 concurrent users initially
- Responsive UI updates

### 5.2 Usability
- Intuitive navigation through project hierarchy
- Clear visual feedback for calculation status
- Minimal clicks to perform common tasks
- Helpful error messages

### 5.3 Reliability
- Data integrity through EF Core transactions
- Graceful error handling
- Validation of calculation inputs

### 5.4 Maintainability
- Clean code architecture
- Well-documented card templates
- Version control integration
- Database migrations for schema changes

### 5.5 Security (Future Phase)
- User authentication
- Role-based access control (Admin, Engineer, Viewer)
- Audit logging
- Data encryption at rest

---

## 6. User Interface Requirements

### 6.1 Navigation Structure
```
Home
├── Projects (List)
│   └── Project Detail
│       └── Jobs (List)
│           └── Job Detail
│               └── Calculations (List)
│                   └── Calculation Detail (Cards)
├── Card Templates (Library)
├── Material Library
└── Settings
```

### 6.2 Project List Page
- Table/grid view of all projects
- Search and filter capabilities
- Quick stats (# of jobs, last modified)
- Create new project button
- Edit/Delete actions

### 6.3 Job Detail Page
- Job information
- Global variables panel (editable)
- List of calculations
- Add calculation button

### 6.4 Calculation Detail Page (Main Workspace)
- Calculation title and revision info
- Card display area (sequential cards)
- Each card shows:
  - Card title
  - MathML formula
  - Input fields with units
  - Calculated outputs with units
  - Design load vs. Capacity comparison
  - Status indicator (color-coded)
  - Verbose toggle
- Add card button (from template library)
- Save revision button
- Export button (future)

### 6.5 Card Template Editor
- Template name and description
- Code editor (C# syntax highlighting)
- MathML input/preview
- Variable definition table
- Design load/capacity specification
- Save/Test buttons

### 6.6 Material Library Browser
- Filter by type (Steel/Concrete/Timber)
- Filter by standard
- Material properties display
- Add to job as global variables

---

## 7. Implementation Strategy

### Phase 1: Foundation (Weeks 1-3)
**Goal:** Establish core infrastructure and data model

**Deliverables:**
1. Docker container setup with VS Code Server
2. Blazor Server project initialization
3. Database creation using EF Core
4. Entity models (Project, Job, Calculation, Card, etc.)
5. Initial migrations
6. Basic CRUD pages for Projects (no authentication)
7. Basic CRUD pages for Jobs
8. Global variable management

**Success Criteria:**
- Database successfully created and migrated
- Can create, view, edit, delete projects and jobs
- Application runs at http://192.168.0.175:5227

### Phase 2: Calculation & Card Structure (Weeks 4-6)
**Goal:** Implement calculation management and basic card functionality

**Deliverables:**
1. Calculation CRUD pages
2. Revision management system
3. Card entity structure
4. Basic card template editor
5. Card instance management
6. Variable injection system (global → local)
7. Simple card display (no calculations yet)

**Success Criteria:**
- Can create calculations with revisions
- Can create and save card templates
- Can add cards to calculations
- Variables properly inherited from job level

### Phase 3: AI Integration (Weeks 7-9)
**Goal:** Enable AI-assisted card generation early for development productivity

**Deliverables:**
1. Terminal interface for AI interaction (Codex/Claude CLI)
2. Engineering text parsing workflow
3. Code generation templates
4. MathML generation from AI
5. Review and approval workflow
6. AI-generated card import process
7. Initial card template examples from SCI_P938.pdf

**Success Criteria:**
- AI can read engineering PDFs (like SCI_P938.pdf)
- AI generates valid C# code for calculations
- AI generates valid MathML formulas
- Developer can review, test, and modify AI output
- Can create 3-5 working card templates using AI assistance

**Rationale:** Moving AI integration earlier allows rapid creation of card templates needed for testing the calculation engine in Phase 4. This accelerates development and provides real-world test cases.

### Phase 4: Calculation Engine (Weeks 10-12)
**Goal:** Implement core calculation functionality using AI-generated cards

**Deliverables:**
1. C# code execution engine for cards
2. Variable substitution system
3. Calculation trigger (on cell exit)
4. Result storage
5. Design load vs. capacity comparison
6. Status determination (Pass/Fail/Warning)
7. Color-coded visual feedback
8. Basic error handling
9. Test with AI-generated card templates

**Success Criteria:**
- Cards calculate correctly when values entered
- Results display with proper color coding
- Design load vs. capacity comparison works
- Calculation results persist
- AI-generated cards execute successfully

### Phase 5: MathML Rendering (Weeks 13-14)
**Goal:** Display mathematical formulas using MathML

**Deliverables:**
1. MathML parser integration
2. Formula display component
3. Variable substitution in formulas
4. Verbose mode (intermediate steps)
5. Formula editor/validator in card template editor
6. Render AI-generated MathML formulas

**Success Criteria:**
- Mathematical formulas render correctly
- Values substitute into formulas properly
- Verbose mode shows intermediate calculations
- Formulas are readable and properly formatted
- AI-generated MathML displays correctly

### Phase 6: Material Library (Weeks 15-16)
**Goal:** Create reusable material property library

**Deliverables:**
1. Material Library entity model
2. Pre-populated material data (AS4100, AS3600 standards)
3. Material browser UI
4. Add material to job functionality
5. Custom material creation

**Success Criteria:**
- Material library contains common materials
- Can browse and search materials
- Can add materials as global variables
- Can create custom materials

### Phase 6: Material Library (Weeks 15-16)
**Goal:** Create reusable material property library

**Deliverables:**
1. Material Library entity model
2. Pre-populated material data (AS4100, AS3600 standards)
3. Material browser UI
4. Add material to job functionality
5. Custom material creation
6. Use AI to help populate library from standards

**Success Criteria:**
- Material library contains common materials
- Can browse and search materials
- Can add materials as global variables
- Can create custom materials

### Phase 7: AI Enhancement & Card Updates (Weeks 17-18)
**Goal:** Expand AI capabilities for card maintenance

**Deliverables:**
1. Card update capability via AI (with review)
2. Bulk card generation workflows
3. AI documentation and best practices
4. Card validation tools
5. Template library expansion using AI

**Success Criteria:**
- AI can update existing cards (with review)
- Update workflow preserves existing functionality
- Template library contains 20+ common calculations
- Documentation enables team to use AI effectively

### Phase 8: PDF Export & Reporting (Weeks 19-20)
**Goal:** Generate calculation reports

**Deliverables:**
1. PDF generation library integration
2. Calculation report template
3. Include formulas, inputs, outputs, results
4. Include company branding (configurable)
5. Export button in UI
6. Excel export option

**Success Criteria:**
- Can generate PDF of full calculation
- PDF includes all cards with formulas
- PDF shows pass/fail status clearly
- Branding appears correctly
- Can export to Excel

### Phase 8: Authentication & Authorization (Weeks 19-21)
**Goal:** Implement user management and security

**Deliverables:**
1. User authentication system
2. Role-based access control (Admin, Engineer, Viewer)
3. User management pages
4. Permission checks on CRUD operations
5. Multi-user collaboration support
6. Audit logging

**Success Criteria:**
- Users must log in to access app
- Admins can manage users and roles
- Engineers can create/edit calculations
- Viewers can only view (no edit)
- Multiple users can work on same project
- Changes are tracked by user

### Phase 9: Backup & Production Hardening (Weeks 22-23)
**Goal:** Prepare for production deployment

**Deliverables:**
1. Automated database backup system
2. Disaster recovery procedures
3. Production reverse proxy configuration (https://app.nedevans.au)
4. Performance optimization
5. Comprehensive error handling
6. User documentation
7. Admin documentation

**Success Criteria:**
- Daily automated backups running
- Recovery tested and documented
- App accessible via https://app.nedevans.au
- Handles 6+ concurrent users smoothly
- All errors logged and handled gracefully
- Documentation complete

### Phase 10: Refinement & Enhancement (Weeks 24+)
**Goal:** Polish and add requested features

**Deliverables:**
1. User feedback incorporation
2. UI/UX improvements
3. Additional card templates
4. Performance optimization
5. Advanced reporting features
6. Integration capabilities

**Success Criteria:**
- User satisfaction high
- No critical bugs
- Performance meets requirements
- Feature requests prioritized and tracked

---

## 8. Key Design Decisions

### 8.1 Why Blazor Server?
- C# throughout stack (backend and frontend)
- Server-side rendering reduces client complexity
- Real-time updates with SignalR
- Easier to secure calculation logic

### 8.2 Why Server-Side Interactive Rendering?
- Immediate feedback for calculations
- Better performance for complex calculations
- Simplified state management
- Reduced data transfer for calculation results

### 8.3 Why EF Core Migrations?
- Version-controlled schema changes
- Easy deployment across environments
- Rollback capability
- Team collaboration on schema changes

### 8.4 Why MathML?
- Web standard for mathematical notation
- Accessible (screen readers)
- Scales well
- Printable in reports
- No external dependencies for rendering

### 8.5 Why Terminal-Based AI?
- Developer maintains control
- Code review enforced
- No auto-deployment risks
- Flexible AI provider (Codex, Claude CLI, etc.)
- Works within existing development workflow

---

## 9. Risks & Mitigations

### 9.1 Risk: Complex Calculation Logic
**Impact:** High  
**Probability:** Medium  
**Mitigation:** 
- Start with simple card examples
- Extensive testing framework
- AI assistance for code generation
- Code review process

### 9.2 Risk: MathML Learning Curve
**Impact:** Medium  
**Probability:** Medium  
**Mitigation:**
- Reference documentation (mathmlv3.pdf)
- AI assistance for generation
- Template library of common formulas
- Visual preview in editor

### 9.3 Risk: Performance with Complex Calculations
**Impact:** Medium  
**Probability:** Low  
**Mitigation:**
- Async calculation execution
- Caching of results
- Optimized database queries
- Profiling and optimization in Phase 9

### 9.4 Risk: Database Connection Security
**Impact:** High  
**Probability:** Low  
**Mitigation:**
- VPN/secure network
- Database-level security
- Connection string encryption
- Regular security audits

### 9.5 Risk: AI-Generated Code Quality
**Impact:** Medium  
**Probability:** Medium  
**Mitigation:**
- Mandatory code review
- Comprehensive testing
- Validation framework
- Human oversight required

---

## 10. Success Metrics

### 10.1 Development Metrics
- Phases completed on schedule
- Code coverage > 70%
- Zero critical bugs in production

### 10.2 User Adoption Metrics
- 6 active users within first month
- Average 10+ calculations per user per week
- User satisfaction score > 8/10

### 10.3 System Metrics
- 99% uptime
- Page load < 2 seconds
- Calculation execution < 500ms
- Zero data loss incidents

### 10.4 Business Metrics
- 50% reduction in calculation time vs. manual methods
- 100+ calculation templates in library within 6 months
- Reuse rate of templates > 60%

---

## 11. Future Enhancements (Post-Launch)

### 11.1 Advanced Features
- Real-time collaboration (multiple users on same calculation)
- Calculation comparison tools
- Advanced search across all calculations
- Mobile-responsive interface
- Offline mode

### 11.2 Integration
- CAD software integration
- BIM model integration
- Document management system integration
- Email notifications for approvals

### 11.3 Analytics
- Usage analytics dashboard
- Most-used calculation types
- Average calculation time
- Error rate tracking

### 11.4 Advanced AI
- AI suggestions for optimization
- Automatic code checking standards compliance
- Natural language query of calculations
- Predictive maintenance of card templates

---

## 12. Appendices

### 12.1 Glossary
- **Card:** Individual engineering check/calculation component
- **Calculation:** Collection of related cards (e.g., "Bolted Moment Connection")
- **Job:** Collection of calculations for a work area (e.g., "Milling Area")
- **Project:** Collection of jobs for an overall project (e.g., "Fimiston Gold")
- **Global Variable:** Variable defined at job level, available to all cards
- **Local Variable:** Variable defined for specific card instance
- **Revision:** Saved version of a calculation
- **MathML:** Mathematical Markup Language for displaying formulas

### 12.2 Reference Standards
- AS4100: Australian Steel Structures Standard
- AS3600: Australian Concrete Structures Standard
- SCI P938: Steel Construction Institute Publication (example reference)

### 12.3 Development Environment
- **Container:** Docker with VS Code Server
- **Port:** 5227 (exposed)
- **URLs:**
  - Development: http://192.168.0.175:5227
  - Production: https://app.nedevans.au
- **Database:** SQL Server 192.168.0.175,1433

---

**Document Control**  
*This PRD is a living document and will be updated as requirements evolve.*