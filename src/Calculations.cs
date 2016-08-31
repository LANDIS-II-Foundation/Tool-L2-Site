using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Landis.Biomass;


namespace SiteVegCalc
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
            
            //  Age-related mortality is discounted from growth-related
            //  mortality to prevent the under-estimation of mortality.  Cannot be negative.
            M_BIO = Math.Max(0, M_BIO - mortalityAge);

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
    }
}
