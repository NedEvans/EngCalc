using EngineeringCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringCalc.Data;

public static class AppConstantSeeder
{
    public static async Task SeedAppConstants(ApplicationDbContext context)
    {
        // Check if already seeded
        if (await context.AppConstants.AnyAsync())
        {
            return; // Already seeded
        }

        var constants = new List<AppConstant>
        {
            // AS4100 - Steel Properties
            new AppConstant
            {
                ConstantName = "fy_Grade250",
                DefaultValue = "250",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel yield strength - Grade 250"
            },
            new AppConstant
            {
                ConstantName = "fy_Grade300",
                DefaultValue = "300",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel yield strength - Grade 300"
            },
            new AppConstant
            {
                ConstantName = "fy_Grade350",
                DefaultValue = "350",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel yield strength - Grade 350"
            },
            new AppConstant
            {
                ConstantName = "fy_Grade400",
                DefaultValue = "400",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel yield strength - Grade 400"
            },
            new AppConstant
            {
                ConstantName = "fy_Grade450",
                DefaultValue = "450",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel yield strength - Grade 450"
            },
            new AppConstant
            {
                ConstantName = "fu_Grade250",
                DefaultValue = "410",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel tensile strength - Grade 250"
            },
            new AppConstant
            {
                ConstantName = "fu_Grade300",
                DefaultValue = "440",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel tensile strength - Grade 300"
            },
            new AppConstant
            {
                ConstantName = "fu_Grade350",
                DefaultValue = "480",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel tensile strength - Grade 350"
            },
            new AppConstant
            {
                ConstantName = "fu_Grade400",
                DefaultValue = "500",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel tensile strength - Grade 400"
            },
            new AppConstant
            {
                ConstantName = "fu_Grade450",
                DefaultValue = "520",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Steel tensile strength - Grade 450"
            },
            new AppConstant
            {
                ConstantName = "Es_Steel",
                DefaultValue = "200000",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Young's modulus for structural steel"
            },
            new AppConstant
            {
                ConstantName = "nu_Steel",
                DefaultValue = "0.3",
                Unit = "",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Poisson's ratio for steel"
            },
            new AppConstant
            {
                ConstantName = "G_Steel",
                DefaultValue = "80000",
                Unit = "MPa",
                Standard = "AS4100",
                Category = "Steel",
                Description = "Shear modulus for steel"
            },

            // AS4100 - Capacity Reduction Factors
            new AppConstant
            {
                ConstantName = "phi_Tension",
                DefaultValue = "0.9",
                Unit = "",
                Standard = "AS4100",
                Category = "Factors",
                Description = "Capacity reduction factor for tension members"
            },
            new AppConstant
            {
                ConstantName = "phi_Compression",
                DefaultValue = "0.9",
                Unit = "",
                Standard = "AS4100",
                Category = "Factors",
                Description = "Capacity reduction factor for compression members"
            },
            new AppConstant
            {
                ConstantName = "phi_Bending",
                DefaultValue = "0.9",
                Unit = "",
                Standard = "AS4100",
                Category = "Factors",
                Description = "Capacity reduction factor for bending members"
            },
            new AppConstant
            {
                ConstantName = "phi_Shear",
                DefaultValue = "0.9",
                Unit = "",
                Standard = "AS4100",
                Category = "Factors",
                Description = "Capacity reduction factor for shear"
            },
            new AppConstant
            {
                ConstantName = "phi_Bearing",
                DefaultValue = "0.9",
                Unit = "",
                Standard = "AS4100",
                Category = "Factors",
                Description = "Capacity reduction factor for bearing"
            },

            // AS3600 - Concrete Properties
            new AppConstant
            {
                ConstantName = "fc_20",
                DefaultValue = "20",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 20 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_25",
                DefaultValue = "25",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 25 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_32",
                DefaultValue = "32",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 32 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_40",
                DefaultValue = "40",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 40 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_50",
                DefaultValue = "50",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 50 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_65",
                DefaultValue = "65",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 65 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_80",
                DefaultValue = "80",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 80 MPa"
            },
            new AppConstant
            {
                ConstantName = "fc_100",
                DefaultValue = "100",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Concrete compressive strength - 100 MPa"
            },
            new AppConstant
            {
                ConstantName = "Ec_20",
                DefaultValue = "24000",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Elastic modulus for 20 MPa concrete"
            },
            new AppConstant
            {
                ConstantName = "Ec_25",
                DefaultValue = "26700",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Elastic modulus for 25 MPa concrete"
            },
            new AppConstant
            {
                ConstantName = "Ec_32",
                DefaultValue = "30100",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Elastic modulus for 32 MPa concrete"
            },
            new AppConstant
            {
                ConstantName = "Ec_40",
                DefaultValue = "32800",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Elastic modulus for 40 MPa concrete"
            },
            new AppConstant
            {
                ConstantName = "nu_Concrete",
                DefaultValue = "0.2",
                Unit = "",
                Standard = "AS3600",
                Category = "Concrete",
                Description = "Poisson's ratio for concrete"
            },

            // AS3600 - Reinforcement
            new AppConstant
            {
                ConstantName = "fsy_N_Class",
                DefaultValue = "500",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Reinforcement",
                Description = "Yield strength for N-class reinforcement"
            },
            new AppConstant
            {
                ConstantName = "fsy_L_Class",
                DefaultValue = "500",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Reinforcement",
                Description = "Yield strength for L-class reinforcement"
            },
            new AppConstant
            {
                ConstantName = "Es_Reinforcement",
                DefaultValue = "200000",
                Unit = "MPa",
                Standard = "AS3600",
                Category = "Reinforcement",
                Description = "Elastic modulus for steel reinforcement"
            },

            // AS3600 - Capacity Reduction Factors
            new AppConstant
            {
                ConstantName = "phi_Concrete_Bending",
                DefaultValue = "0.85",
                Unit = "",
                Standard = "AS3600",
                Category = "Factors",
                Description = "Capacity reduction factor for concrete in bending"
            },
            new AppConstant
            {
                ConstantName = "phi_Concrete_Compression",
                DefaultValue = "0.65",
                Unit = "",
                Standard = "AS3600",
                Category = "Factors",
                Description = "Capacity reduction factor for concrete in compression"
            },
            new AppConstant
            {
                ConstantName = "phi_Concrete_Shear",
                DefaultValue = "0.7",
                Unit = "",
                Standard = "AS3600",
                Category = "Factors",
                Description = "Capacity reduction factor for concrete in shear"
            },

            // Common Load Factors
            new AppConstant
            {
                ConstantName = "gamma_G",
                DefaultValue = "1.2",
                Unit = "",
                Standard = "AS1170.0",
                Category = "Load Factors",
                Description = "Dead load factor (permanent actions)"
            },
            new AppConstant
            {
                ConstantName = "gamma_Q",
                DefaultValue = "1.5",
                Unit = "",
                Standard = "AS1170.0",
                Category = "Load Factors",
                Description = "Live load factor (imposed actions)"
            },
            new AppConstant
            {
                ConstantName = "psi_s",
                DefaultValue = "0.7",
                Unit = "",
                Standard = "AS1170.0",
                Category = "Load Factors",
                Description = "Short-term combination factor"
            },
            new AppConstant
            {
                ConstantName = "psi_l",
                DefaultValue = "0.4",
                Unit = "",
                Standard = "AS1170.0",
                Category = "Load Factors",
                Description = "Long-term combination factor"
            }
        };

        await context.AppConstants.AddRangeAsync(constants);
        await context.SaveChangesAsync();
    }
}
