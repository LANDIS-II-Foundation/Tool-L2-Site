using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Landis.Biomass;


namespace L2_Site_Budworm
{
    public class Calculations
    {

        static public double ComputeActualANPPV2(double B_AP, double B_PM, int maxANPP)
        {

            double actualANPP = 0;
            if (B_PM > 0)
            {
                actualANPP = maxANPP * Math.E * B_AP * Math.Exp(-1 * B_AP) * B_PM;
            }
            // Calculated actual ANPP can not exceed the limit set by the
            //  maximum ANPP times the ratio of potential to maximum biomass.
            //  This down regulates actual ANPP by the available growing space.
            actualANPP = Math.Min(maxANPP * B_PM, actualANPP);
  
            return actualANPP;
        }

        static public double ComputePropANPP(double B_AP,double B_PM, double growthShape)
        {
            // Matches BS 3.5
            double propANPP = Math.E * Math.Pow((B_AP), growthShape) * Math.Exp(-1 * Math.Pow((B_AP), growthShape)) * B_PM;
            return propANPP;
        }

        static public double ComputeAgeMortalityProp(double X0, double mortR, double age)
        {
            // Matches BS 3.5
            double prop = 1/(1+((1/  X0)-1)*Math.Exp(-1*mortR*age));
            return prop;
        }
        static public double ComputeAgeMortalityPropV2(double d, double max_age, double age)
        {
            double prop =  Math.Exp(age / max_age * d) / Math.Exp(d);

            return prop;
        }
        static public double Compute_X0(int longevity, double paramD)
        {
            double t1 = (double)longevity * paramD;
            double p1 = 0.05;
            int t2 = longevity;
            double p2 = 0.95;
            // Matches BS 3.5
            double X0 = (Math.Exp(t1 * ((Math.Log((p2 - 1) * p1 / ((p1 - 1) * p2))) / (-1 * t1 + t2)))) * p1 / ((Math.Exp(t1 * ((Math.Log((p2 - 1) * p1 / ((p1 - 1) * p2))) / (-1 * t1 + t2)))) * p1 - p1 + 1);
            return X0;
        }
        static public double Compute_mortR(double X0, int longevity, double paramD)
        {
            double t1 = (double)longevity * paramD;
            double p1 = 0.05;
            // Matches BS 3.5
            double mortR = (-1 * Math.Log(X0 * ((p1 - 1) / (p1 * (-1 + X0))))) / t1;
            return mortR;
        }
        static public double ComputeGrowthMortalityV2(double maxANPP, double paramY, double paramR, double B_AP, double B_PM, double bio, double mortalityAge, double actualANPP)
        {
            //  Growth-related mortality
            double mortalityGrowth = Math.Min(Math.Min(maxANPP * (paramY / (paramY + (1 - paramY) * Math.Exp(-1 * paramR * Math.Pow(paramY, -1) * B_AP))) * B_PM, bio), maxANPP * B_PM);
            //  Age-related mortality is discounted from growth-related
            //  mortality to prevent the under-estimation of mortality.  Cannot be negative.
            mortalityGrowth = Math.Max(0, mortalityGrowth - mortalityAge);
            //  Also ensure that growth mortality does not exceed actualANPP.
            mortalityGrowth = Math.Min(mortalityGrowth, actualANPP);
            return mortalityGrowth;
        }
        static public double ComputeGrowthMortalityV3(double actualANPP, double maxANPP, double B_AP, double B_PM, double bio, double mortalityAge, double growthReduction)
        {
            //  Growth-related mortality
            double M_BIO = 0.0;

            //Michaelis-Menton function:
            if (B_AP > 1.0)
                M_BIO = maxANPP * B_PM;
            else
                M_BIO = maxANPP * (2.0 * B_AP) / (1.0 + B_AP) * B_PM;

            //  Mortality should not exceed the amount of living biomass
            M_BIO = Math.Min(bio, M_BIO);

            //  Calculated actual ANPP can not exceed the limit set by the
            //  maximum ANPP times the ratio of potential to maximum biomass.
            //  This down regulates actual ANPP by the available growing space.

            M_BIO = Math.Min(maxANPP * B_PM, M_BIO);

            if (growthReduction > 0)
                M_BIO *= (1.0 - growthReduction);

            return M_BIO;
        }
        static public double ComputeGrowthMortality(double actualANPP, double maxANPP, double B_AP, double B_PM, double bio, double mortalityAge, double thinCoeff)
        {
            //  Growth-related mortality
            //  Matches BS 3.5

            double M_BIO = 1.0;
            // Michaelis-Menton function:
            M_BIO = actualANPP * (2.0 * B_AP) / (1.0 + B_AP) * B_PM;
            //  Mortality should not exceed the amount of living biomass
            M_BIO = Math.Min(bio, M_BIO);

            return M_BIO;
        }

        public static double CalculateCompetition( ICohort cohort, List<Cohort> cohortList)
        {
            double competitionPower = 0.95;
            double CMultiplier = Math.Max(Math.Pow(cohort.Biomass, competitionPower), 1.0);
            double CMultTotal = CMultiplier;
            // PlugIn.ModelCore.Log.WriteLine("Competition:  spp={0}, age={1}, CMultiplier={2:0}, CMultTotal={3:0}.", cohort.Species.Name, cohort.Age, CMultiplier, CMultTotal);

            

                foreach (ICohort xcohort in cohortList)
                {
                    if (xcohort.Age != cohort.Age || xcohort.Species.Index != cohort.Species.Index)
                    {
                        double tempMultiplier = Math.Max(Math.Pow(xcohort.Biomass, competitionPower), 1.0);
                        CMultTotal += tempMultiplier;
                    }
                }
            


            double Cfraction = CMultiplier / CMultTotal;
            //PlugIn.ModelCore.Log.WriteLine("Competition:  spp={0}, age={1}, CMultiplier={2:0}, CMultTotal={3:0}, CI={4:0.00}.", cohort.Species.Name, cohort.Age, CMultiplier, CMultTotal, Cfraction);

            return Cfraction;


        }

        //Periodic (5 year) Mortality Rate = Intercept + AGE + AGE*CD + CD^2 + CD^3
        static double[] params_MR_AGE_BF = new double[] { 0.1182, -0.003634, 0.0001112, -0.0001563, 0.000002064 };
        static double[] params_MR_AGE_BS = new double[] { 0.1509, -0.002707, 0.00007719, -0.0001869, 0.000002194 };
        static double[] params_MR_AGE_RS = new double[] { 0.1509, -0.002707, 0.00007719, -0.0001869, 0.000002194 };
        static double[] params_MR_AGE_WS = new double[] { 0.1005, -0.002187, 0.00006505, -0.0001375, 0.000001891 };
        static double[] params_MR_AGE_NONHOST = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0 };

        /// <summary>
        /// Predicts additive probability of tree death in current simulation year
        /// as a function of <paramref name="host"/>, <paramref name="PCD"/>, and <paramref name="age"/>.
        /// </summary>
        /// <param name="host">SBW host species.</param>
        /// <param name="PCD">
        /// Periodic (5-year) average cumulative defoliation 
        /// as defined by MacLean et al. (2001).
        /// </param>
        /// <param name="age">Tree age.</param>
        /// <returns>Additive tree mortality rate (probability of death in current year).</returns>
        public static double GetMortalityRate_AGE(Landis.Species.ISpecies species, double PCD, int age)
        {


            if (PCD < 0.35)
                return 0; //Avoid extrapolation issues.
            else
                PCD *= 100; //Convert units to percent

            double rate = 0;
            double[] b = null;

            if (species.Name == "abiebals")
            {
                b = params_MR_AGE_BF;
                if (age > 90) age = 90; //Cap to avoid extrapolation issues.
            }
            else if(species.Name == "picemari")
            {
                b = params_MR_AGE_BS;
                if (age > 125) age = 125;
            }
            else if (species.Name == "picerube")
            {
                b = params_MR_AGE_RS;
                if (age > 125) age = 125;
            }
            else if (species.Name == "piceglau")
            {
                b = params_MR_AGE_WS;
                if (age > 125) age = 125;
            }
            else
            {
                b = params_MR_AGE_NONHOST;
            }


            /*switch (species)
            {

                case SbwHosts.BF:
                    b = params_MR_AGE_BF;
                    if (age > 90) age = 90; //Cap to avoid extrapolation issues.
                    break;

                case SbwHosts.BS:
                    b = params_MR_AGE_BS;
                    if (age > 125) age = 125;
                    break;

                case SbwHosts.RS:
                    b = params_MR_AGE_RS;
                    if (age > 125) age = 125;
                    break;

                case SbwHosts.WS:
                    b = params_MR_AGE_WS;
                    if (age > 125) age = 125;
                    break;

                default:
                    throw new ArgumentException("Unknown host species.");
            }
             * */

            //Periodic (5 year) Mortality Rate = Intercept + AGE + CD*AGE + CD^2 + CD^3
            rate = b[0] +
                   b[1] * age +
                   b[2] * age * PCD +
                   b[3] * PCD * PCD +
                   b[4] * PCD * PCD * PCD;

            //Trap out of bounds extrapolated predictions. 
            if (rate < 0)
                return 0;
            else if (rate > 1)
                return 1;

            //Convert from periodic (5 year) to annual mortality rate using the reverse compound interest formula
            //(original fit was based on 5 year mortality rates, so we need to first predict periodic, then convert to annual).
            rate = 1 - Math.Pow(1 - rate, 1 / 5.0);

            return rate;
        }

        public static double HillFunction(double shape, double halfSat, double x)
        {
            double y = Math.Pow(x, shape) / (Math.Pow(halfSat, shape) + Math.Pow(x, shape));
            return y;
        }
        // 2-parameter mating effect - no longer used
        public static double ProportionMatedFunction(double a, double b, double x)
        {
            double y = 1.0 - Math.Exp(-1.0 * a * x - b);
            return y;
        }
        // 3-parameter mating effect
        public static double ProportionMatedFunction(double a, double b, double c, double x)
        {
            double y = 1.0 - Math.Exp(-1.0 * Math.Pow(a,c) * x - b);
            return y;
        }
        public static double MinNonZeroInArray(double[] inputArray)
        {
            double min = 0;
            for (int i = 0; i < inputArray.Length; i++)
            {
                if (inputArray[i] > 0)
                {
                    min = inputArray[i];
                    break;
                }
            }
            for (int i = 0; i < inputArray.Length; i++)
            {
                if (inputArray[i] < min && inputArray[i] > 0)
                {
                    min = inputArray[i];
                }
            }
            return min;
        }
    }
}
