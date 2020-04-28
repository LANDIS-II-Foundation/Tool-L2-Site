using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using ZedGraph;
using Landis.Biomass;
using Landis.Species;
using Troschuetz.Random;

namespace L2_Site_Budworm
{
    public partial class Form1 : Form
    {
        int repCount = 1;
        int rangeCount = 0;
        static bool shrub1 = false;
        static bool shrub2 = false;
        static bool shrub3 = false;
        static bool shrub4 = false;
        static bool shrub5 = false;
        static bool shrub6 = false;
        static double effAge1 = 1;
        static double effAge2 = 1;
        static double effAge3 = 1;
        static double effAge4 = 1;
        static double effAge5 = 1;
        static double effAge6 = 1;
        static int numSpecies = 1;
        string dirName = "";
        string baseFileName = "";


        public Form1()
        {
            InitializeComponent();
        }

        public void buttonRUN_Click(object sender, EventArgs e)
        {
                bool errorCheck = true;
                string mesg = "";
                int randSeed = Environment.TickCount;
                if (cbRandSeed.Checked && !(menuBatchMode.Checked))
                    randSeed = int.Parse(tbRandSeed.Text);
                else
                    tbRandSeed.Text = randSeed.ToString();
                Troschuetz.Random.StandardGenerator randGen = new Troschuetz.Random.StandardGenerator(randSeed);

                // General parameters
                string executableName = Application.ExecutablePath;
                FileInfo executableFileInfo = new FileInfo(executableName);
               dirName = executableFileInfo.DirectoryName;
            
                if (cbOutputFolder.Checked)
                {
                    dirName = tbOutputFolder.Text;
                    System.IO.Directory.CreateDirectory(dirName);
                }
                baseFileName = tbBaseFileName.Text;
                int timestep = int.Parse(tbTimestep.Text);
                int simYears = int.Parse(tbSimYears.Text);
                double paramY = 0.01;
                double paramR = 0.08;
                double leafFraction = 0.35;

                double shade1 = 0;
                double shade2 = 0;
                double shade3 = 0;
                double shade4 = 0;
                double shade5 = 0;
                List<List<double>> sufficientLight = new List<List<double>>();
                List<double> light1 = new List<double>();
                List<double> light2 = new List<double>();
                List<double> light3 = new List<double>();
                List<double> light4 = new List<double>();
                List<double> light5 = new List<double>();
                double light01 = 0;
                double light02 = 0;
                double light03 = 0;
                double light04 = 0;
                double light05 = 0;
                double light11 = 0;
                double light12 = 0;
                double light13 = 0;
                double light14 = 0;
                double light15 = 0;
                double light21 = 0;
                double light22 = 0;
                double light23 = 0;
                double light24 = 0;
                double light25 = 0;
                double light31 = 0;
                double light32 = 0;
                double light33 = 0;
                double light34 = 0;
                double light35 = 0;
                double light41 = 0;
                double light42 = 0;
                double light43 = 0;
                double light44 = 0;
                double light45 = 0;
                double light51 = 0;
                double light52 = 0;
                double light53 = 0;
                double light54 = 0;
                double light55 = 0;

                //Budworm - Declare input parameters
                int budwormStartYear = 0;
                double budwormrm = 0;
                double budwormrprimem = 0;
                double budwormb = 0;
                double budwormcprime = 0;
                double budwormbprime = 0;
                double budwormc = 0;
                double budEnemyDensity0 = 0;
                double budwormDensity0 = 0;
                double budwormMaxDensity = 0;
                double budwormbprime2 = 0;
                double budwormaprime2 = 0;
                double budwormaprime = 0;
                double budwormdeltaaprime2 = 0;
                double budwormdeltabprime2 = 0;
                double budwormWinterMean = 1;
                double budwormWinterStdev = 0;
                double budwormPredationMean = 1;
                double budwormPredationStdev = 0;
                bool budwormWinterConstant = false;
                bool budwormPredationConstant = false;
                Troschuetz.Random.NormalDistribution normDistWinter = new Troschuetz.Random.NormalDistribution(randGen);
                Troschuetz.Random.NormalDistribution normDistPredation = new Troschuetz.Random.NormalDistribution(randGen);
                Troschuetz.Random.BetaDistribution betaDistDefol = new Troschuetz.Random.BetaDistribution(randGen);
                bool budwormHost1 = false;
                bool budwormHost2 = false;
                bool budwormHost3 = false;
                bool budwormHost4 = false;
                bool budwormHost5 = false;
                bool budwormHost6 = false;
                bool budwormDecid1 = false;
                bool budwormDecid2 = false;
                bool budwormDecid3 = false;
                bool budwormDecid4 = false;
                bool budwormDecid5 = false;
                bool budwormDecid6 = false;
                bool budwormCondHost1 = false;
                bool budwormCondHost2 = false;
                bool budwormCondHost3 = false;
                bool budwormCondHost4 = false;
                bool budwormCondHost5 = false;
                bool budwormCondHost6 = false;
                double matingA = 0;
                double matingB = 0;
                double matingC = 0;
                double decidProtectDmax1 = 0;
                double decidProtectDmax2 = 0;
                double phenolLimitMean = 1;
                double phenolLimitStdev = 0;
                bool phenolLimitConstant = false;
                Troschuetz.Random.NormalDistribution normDistPhenolLimit = new Troschuetz.Random.NormalDistribution(randGen);
                double defolLambda = 0.255;
                double minLDD = 0;
                double halfLDD = 0;
                double maxLDD = 0;
                double maxLDDProp = 0;
                int minSuscAge = 0;
                double predM = 1.0;
                double predN = 1.0;
                double preyM = 1.0;
                double preyN = 1.0;
            //Budworm - END - Declare input parameters

                if (rbV30.Checked || rbV35.Checked)
                {
                    shade1 = double.Parse(tbRelBio1.Text);
                    shade2 = double.Parse(tbRelBio2.Text);
                    shade3 = double.Parse(tbRelBio3.Text);
                    shade4 = double.Parse(tbRelBio4.Text);
                    shade5 = double.Parse(tbRelBio5.Text);

                    light01 = double.Parse(tbLight01.Text);
                    light02 = double.Parse(tbLight02.Text);
                    light03 = double.Parse(tbLight03.Text);
                    light04 = double.Parse(tbLight04.Text);
                    light05 = double.Parse(tbLight05.Text);
                    light11 = double.Parse(tbLight11.Text);
                    light12 = double.Parse(tbLight12.Text);
                    light13 = double.Parse(tbLight13.Text);
                    light14 = double.Parse(tbLight14.Text);
                    light15 = double.Parse(tbLight15.Text);
                    light21 = double.Parse(tbLight21.Text);
                    light22 = double.Parse(tbLight22.Text);
                    light23 = double.Parse(tbLight23.Text);
                    light24 = double.Parse(tbLight24.Text);
                    light25 = double.Parse(tbLight25.Text);
                    light31 = double.Parse(tbLight31.Text);
                    light32 = double.Parse(tbLight32.Text);
                    light33 = double.Parse(tbLight33.Text);
                    light34 = double.Parse(tbLight34.Text);
                    light35 = double.Parse(tbLight35.Text);
                    light41 = double.Parse(tbLight41.Text);
                    light42 = double.Parse(tbLight42.Text);
                    light43 = double.Parse(tbLight43.Text);
                    light44 = double.Parse(tbLight44.Text);
                    light45 = double.Parse(tbLight45.Text);
                    light51 = double.Parse(tbLight51.Text);
                    light52 = double.Parse(tbLight52.Text);
                    light53 = double.Parse(tbLight53.Text);
                    light54 = double.Parse(tbLight54.Text);
                    light55 = double.Parse(tbLight55.Text);

                    light1.Add(light01);
                    light1.Add(light11);
                    light1.Add(light21);
                    light1.Add(light31);
                    light1.Add(light41);
                    light1.Add(light51);
                    light2.Add(light02);
                    light2.Add(light12);
                    light2.Add(light22);
                    light2.Add(light32);
                    light2.Add(light42);
                    light2.Add(light52);
                    light3.Add(light03);
                    light3.Add(light13);
                    light3.Add(light23);
                    light3.Add(light33);
                    light3.Add(light43);
                    light3.Add(light53);
                    light4.Add(light04);
                    light4.Add(light14);
                    light4.Add(light24);
                    light4.Add(light34);
                    light4.Add(light44);
                    light4.Add(light54);
                    light5.Add(light05);
                    light5.Add(light15);
                    light5.Add(light25);
                    light5.Add(light35);
                    light5.Add(light45);
                    light5.Add(light55);

                    sufficientLight.Add(light1);
                    sufficientLight.Add(light2);
                    sufficientLight.Add(light3);
                    sufficientLight.Add(light4);
                    sufficientLight.Add(light5);

                    //Budworm - Read parameter values from text boxes
                    budwormStartYear = int.Parse(tbBudStartYear.Text);
                    budwormrm = double.Parse(tbBudrm.Text);
                    budwormrprimem = double.Parse(tbBudrprimem.Text);
                    budwormb = double.Parse(tbBudb.Text);
                    budwormbprime = double.Parse(tbBudbprime.Text);
                    budwormcprime = double.Parse(tbBudcprime.Text);
                    budwormc = double.Parse(tbBudc.Text);
                    budEnemyDensity0 = double.Parse(tbBudx0.Text);
                    budwormDensity0 = double.Parse(tbBudy0.Text);
                    budwormMaxDensity = double.Parse(tbBudMaxDensity.Text);
                    budwormaprime = double.Parse(tbBudaprime.Text);
                    budwormWinterMean = double.Parse(tbBudWinterMean.Text);
                    budwormWinterStdev = double.Parse(tbBudWinterStdev.Text);
                    budwormPredationMean = double.Parse(tbBudPredationMean.Text);
                    budwormPredationStdev = double.Parse(tbBudPredationStdev.Text);
                    matingA = double.Parse(tbAlleeShape.Text);
                    matingB = double.Parse(tbAlleeScale.Text);
                    matingC = double.Parse(tbMatingC.Text);
                    decidProtectDmax1 = double.Parse(tbDecidProtectD1.Text);
                    decidProtectDmax2 = double.Parse(tbDecidProtectD2.Text);
                    phenolLimitMean = double.Parse(tbBudPhenolMean.Text);
                    phenolLimitStdev = double.Parse(tbBudPhenolStdev.Text);
                    defolLambda = double.Parse(tbDefolLambda.Text);
                    minLDD = double.Parse(tbMinLDD.Text);
                    halfLDD = double.Parse(tbHalfLDD.Text);
                    maxLDD = double.Parse(tbMaxLDD.Text);
                    maxLDDProp = double.Parse(tbMaxLDDProp.Text);
                    minSuscAge = int.Parse(tbMinSuscAge.Text);
                    predM = double.Parse(tbPredM.Text);
                    predN = double.Parse(tbPredN.Text);
                    preyM = double.Parse(tbPreyM.Text);
                    preyN = double.Parse(tbPreyN.Text);

                    // the normal distribution will not accept a sigma value of 0
                    // we want to use a constant equal to the mean (mu) when stdev = 0
                    // use the *Constant boolean parameters to control this
                    // 
                    if (budwormWinterStdev == 0)
                        budwormWinterConstant = true;
                    else
                    {
                        normDistWinter.Mu = budwormWinterMean;
                        normDistWinter.Sigma = budwormWinterStdev;
                    }
                    if (budwormPredationStdev == 0)
                        budwormPredationConstant = true;
                    else
                    {
                        normDistPredation.Mu = budwormPredationMean;
                        normDistPredation.Sigma = budwormPredationStdev;
                    }
                    if (phenolLimitStdev == 0)
                        phenolLimitConstant = true;
                    else
                    {
                        normDistPhenolLimit.Mu = phenolLimitMean;
                        normDistPhenolLimit.Sigma = phenolLimitStdev;
                    }
                }

                //Budworm - END - Read parameter values from text boxes

                int sppNum1 = int.Parse(tbSppNum1.Text);
                int sppNum2 = int.Parse(tbSppNum2.Text);
                int sppNum3 = int.Parse(tbSppNum3.Text);
                int sppNum4 = int.Parse(tbSppNum4.Text);
                int sppNum5 = int.Parse(tbSppNum5.Text);
                int sppNum6 = int.Parse(tbSppNum6.Text);

                numSpecies = int.Parse(tbNumberSpp.Text);
                
                errorCheck = CheckSppNum(sppNum1, sppNum2, sppNum3, sppNum4, sppNum5,sppNum6);
                if (!errorCheck)
                    mesg = "Species must be numbered 1, 2, 3, 4, 5 and 6.";
                string sppName = "";
                int longevity1 = 0;
                int shadeTol1 = 0;
                int maxANPP1 = 0;
                int shadeANPP1 = 0;
                int maxBiomass = 0;
                double paramD1 = 0;
                double sppEstab = 0;
                double maxLAI1 = 0;
                double paramK1 = 0;
                double maturityAge1 = 0;
                double leafLongevity = 0;
                int seedYear1 = 0;
                int plantYear1 = 0;
                int removeYear1 = 0;
                double removeProp1 = 0;
                double anppPower1 = 0;
                double decayRate1 = 0;
                double X0_1 = 0;
                double mortR_1 = 0;
                bool rangeANPP1 = false;
                bool rangeAP1 = false;
                bool rangeD1 = false;
                bool rangeMM1 = false;
                double anppPeak1 = 1;
                double mortMod1 = 1;
                double thinCoeff1 = 1;
                double resproutProb1 = 0.0;
                int minVegAge1 = 0;
                int maxVegAge1 = 0;
                string sppName2 = "";
                int longevity2 = 0;
                int shadeTol2 = 0;
                int maxANPP2 = 0;
                int shadeANPP2 = 0;
                int maxBiomass2 = 0;
                double paramD2 = 0;
                double sppEstab2 = 0;
                double maxLAI2 = 0;
                double paramK2 = 0;
                double maturityAge2 = 0;
                double leafLongevity2 = 0;
                int seedYear2 = 0;
                int plantYear2 = 0;
                int removeYear2 = 0;
                double removeProp2 = 0;
                double anppPower2 = 0;
                double decayRate2 = 0;
                double X0_2 = 0;
                double mortR_2 = 0;
                bool rangeANPP2 = false;
                bool rangeAP2 = false;
                bool rangeD2 = false;
                bool rangeMM2 = false;
                double anppPeak2 = 1;
                double mortMod2 = 1;
                double thinCoeff2 = 1;
                double resproutProb2 = 0.0;
                int minVegAge2 = 0;
                int maxVegAge2 = 0;
                string sppName3 = "";
                int longevity3 = 0;
                int shadeTol3 = 0;
                int maxANPP3 = 0;
                int shadeANPP3 = 0;
                int maxBiomass3 = 0;
                double paramD3 = 0;
                double sppEstab3 = 0;
                double maxLAI3 = 0;
                double paramK3 = 0;
                double maturityAge3 = 0;
                double leafLongevity3 = 0;
                int seedYear3 = 0;
                int plantYear3 = 0;
                int removeYear3 = 0;
                double removeProp3 = 0;
                double anppPower3 = 0;
                double decayRate3 = 0;
                double X0_3 = 0;
                double mortR_3 = 0;
                bool rangeANPP3 = false;
                bool rangeAP3 = false;
                bool rangeD3 = false;
                bool rangeMM3 = false;
                double anppPeak3 = 1;
                double mortMod3 = 1;
                double thinCoeff3 = 1;
                double resproutProb3 = 0.0;
                int minVegAge3 = 0;
                int maxVegAge3 = 0;
                string sppName4 = "";
                int longevity4 = 0;
                int shadeTol4 = 0;
                int maxANPP4 = 0;
                int shadeANPP4 = 0;
                int maxBiomass4 = 0;
                double paramD4 = 0;
                double sppEstab4 = 0;
                double maxLAI4 = 0;
                double paramK4 = 0;
                double maturityAge4 = 0;
                double leafLongevity4 = 0;
                int seedYear4 = 0;
                int plantYear4 = 0;
                int removeYear4 = 0;
                double removeProp4 = 0;
                double anppPower4 = 0;
                double decayRate4 = 0;
                double X0_4 = 0;
                double mortR_4 = 0;
                bool rangeANPP4 = false;
                bool rangeAP4 = false;
                bool rangeD4 = false;
                bool rangeMM4 = false;
                double anppPeak4 = 1;
                double mortMod4 = 1;
                double thinCoeff4 = 1;
                double resproutProb4 = 0.0;
                int minVegAge4 = 0;
                int maxVegAge4 = 0;
                string sppName5 = "";
                int longevity5 = 0;
                int shadeTol5 = 0;
                int maxANPP5 = 0;
                int shadeANPP5 = 0;
                int maxBiomass5 = 0;
                double paramD5 = 0;
                double sppEstab5 = 0;
                double maxLAI5 = 0;
                double paramK5 = 0;
                double maturityAge5 = 0;
                double leafLongevity5 = 0;
                int seedYear5 = 0;
                int plantYear5 = 0;
                int removeYear5 = 0;
                double removeProp5 = 0;
                double anppPower5 = 0;
                double decayRate5 = 0;
                double X0_5 = 0;
                double mortR_5 = 0;
                bool rangeANPP5 = false;
                bool rangeAP5 = false;
                bool rangeD5 = false;
                bool rangeMM5 = false;
                double anppPeak5 = 1;
                double mortMod5 = 1;
                double thinCoeff5 = 1;
                double resproutProb5 = 0.0;
                int minVegAge5 = 0;
                int maxVegAge5 = 0;
                string sppName6 = "";
                int longevity6 = 0;
                int shadeTol6 = 0;
                int maxANPP6 = 0;
                int shadeANPP6 = 0;
                int maxBiomass6 = 0;
                double paramD6 = 0;
                double sppEstab6 = 0;
                double maxLAI6 = 0;
                double paramK6 = 0;
                double maturityAge6 = 0;
                double leafLongevity6 = 0;
                int seedYear6 = 0;
                int plantYear6 = 0;
                int removeYear6 = 0;
                double removeProp6 = 0;
                double anppPower6 = 0;
                double decayRate6 = 0;
                double X0_6 = 0;
                double mortR_6 = 0;
                bool rangeANPP6 = false;
                bool rangeAP6 = false;
                bool rangeD6 = false;
                bool rangeMM6 = false;
                double anppPeak6 = 1;
                double mortMod6 = 1;
                double thinCoeff6 = 1;
                double resproutProb6 = 0.0;
                int minVegAge6 = 0;
                int maxVegAge6 = 0;
                double siteMaxBio = 0;
                int landMaxBio = int.Parse(tbLandMaxBio.Text);
                int maxShadeBiomass = 0;
                int batchNum = 1;
                if (menuBatchMode.Checked)
                    batchNum = int.Parse(tbBatch.Text);


                List<ISpecies> speciesList = new List<ISpecies>();
                //Budworm
                List<ISpecies> hostSpeciesList = new List<ISpecies>();
                List<ISpecies> condHostSpeciesList = new List<ISpecies>();
                //Species 1
                if (sppNum1 == 1)
                {
                    sppName = tbSppName.Text;
                    longevity1 = int.Parse(tbLongevity.Text);
                    shadeTol1 = int.Parse(tbShadeTol.Text);
                    maturityAge1 = double.Parse(tbMatAge1.Text);
                    maxANPP1 = int.Parse(tbANPPmax.Text);
                    maxBiomass = int.Parse(tbBioMax.Text);
                    if(rbV35.Checked)
                        paramD1 = double.Parse(tbAgeMort1.Text);
                    else
                        paramD1 = double.Parse(tbMortShape1.Text);
                    sppEstab = double.Parse(tbSEC.Text);
                    leafLongevity = double.Parse(tbLeaf.Text);
                    seedYear1 = int.Parse(tbSeedYear1.Text);
                    plantYear1 = int.Parse(tbPlant1.Text);
                    removeYear1 = int.Parse(tbRemove1.Text);
                    removeProp1 = double.Parse(tbRemProp1.Text);
                    anppPower1 = double.Parse(tbPower1.Text);
                    decayRate1 = double.Parse(tbDecay1.Text);
                    if (rbV35.Checked)
                        mortMod1 = double.Parse(tbMortMod1.Text);
                    if (cbANPP1.Checked)
                        rangeANPP1 = true;
                    if (cbRangeAP1.Checked)
                        rangeAP1 = true;
                    if (cbRangeAgeMort1.Checked)
                        rangeD1 = true;
                    if (cbRangeMM1.Checked)
                        rangeMM1 = true;
                    resproutProb1 = double.Parse(tbVegProb1.Text);
                    minVegAge1 = int.Parse(tbMinVegAge1.Text);
                    maxVegAge1 = int.Parse(tbMaxVegAge1.Text);
                    budwormHost1 = rbHost1.Checked;
                    budwormDecid1 = rbDecid1.Checked;
                    budwormCondHost1 = rbCondHost1.Checked;
                }
                else if (sppNum2 == 1)
                {
                    sppName = tbSppName2.Text;
                    longevity1 = int.Parse(tbLongevity2.Text);
                    shadeTol1 = int.Parse(tbShadeTol2.Text);
                    maturityAge1 = double.Parse(tbMatAge2.Text);
                    maxANPP1 = int.Parse(tbANPPmax2.Text);
                    maxBiomass = int.Parse(tbBioMax2.Text);
                    if (rbV35.Checked)
                        paramD1 = double.Parse(tbAgeMort2.Text);
                    else
                        paramD1 = double.Parse(tbMortShape2.Text);
                    sppEstab = double.Parse(tbSEC2.Text);
                    leafLongevity = double.Parse(tbLeaf2.Text);
                    seedYear1 = int.Parse(tbSeedYear2.Text);
                    plantYear1 = int.Parse(tbPlant2.Text);
                    removeYear1 = int.Parse(tbRemove2.Text);
                    removeProp1 = double.Parse(tbRemProp2.Text);
                    anppPower1 = double.Parse(tbPower2.Text);
                    decayRate1 = double.Parse(tbDecay2.Text);
                    if (rbV35.Checked)
                        mortMod1 = double.Parse(tbMortMod2.Text);
                    if (cbANPP2.Checked)
                        rangeANPP1 = true;
                    if (cbRangeAP2.Checked)
                        rangeAP1 = true;
                    if (cbRangeAgeMort2.Checked)
                        rangeD1 = true;
                    if (cbRangeMM2.Checked)
                        rangeMM1 = true;
                    resproutProb1 = double.Parse(tbVegProb2.Text);
                    minVegAge1 = int.Parse(tbMinVegAge2.Text);
                    maxVegAge1 = int.Parse(tbMaxVegAge2.Text);
                    budwormHost1 = rbHost2.Checked;
                    budwormDecid1 = rbDecid2.Checked;
                    budwormCondHost1 = rbCondHost2.Checked;
                }
                else if (sppNum3 == 1)
                {
                    sppName = tbSppName3.Text;
                    longevity1 = int.Parse(tbLongevity3.Text);
                    shadeTol1 = int.Parse(tbShadeTol3.Text);
                    maturityAge1 = double.Parse(tbMatAge3.Text);
                    maxANPP1 = int.Parse(tbANPPmax3.Text);
                    maxBiomass = int.Parse(tbBioMax3.Text);
                    if (rbV35.Checked)
                        paramD1 = double.Parse(tbAgeMort3.Text);
                    else
                        paramD1 = double.Parse(tbMortShape3.Text);
                    sppEstab = double.Parse(tbSEC3.Text);
                    leafLongevity = double.Parse(tbLeaf3.Text);
                    seedYear1 = int.Parse(tbSeedYear3.Text);
                    plantYear1 = int.Parse(tbPlant3.Text);
                    removeYear1 = int.Parse(tbRemove3.Text);
                    removeProp1 = double.Parse(tbRemProp3.Text);
                    anppPower1 = double.Parse(tbPower3.Text);
                    decayRate1 = double.Parse(tbDecay3.Text);
                    if (rbV35.Checked)
                        mortMod1 = double.Parse(tbMortMod3.Text);
                    if (cbANPP3.Checked)
                        rangeANPP1 = true;
                    if (cbRangeAP3.Checked)
                        rangeAP1 = true;
                    if (cbRangeAgeMort3.Checked)
                        rangeD1 = true;
                    if (cbRangeMM3.Checked)
                        rangeMM1 = true;
                    resproutProb1 = double.Parse(tbVegProb3.Text);
                    minVegAge1 = int.Parse(tbMinVegAge3.Text);
                    maxVegAge1 = int.Parse(tbMaxVegAge3.Text);
                    budwormHost1 = rbHost3.Checked;
                    budwormDecid1 = rbDecid3.Checked;
                    budwormCondHost1 = rbCondHost3.Checked;
                }
                else if (sppNum4 == 1)
                {
                    sppName = tbSppName4.Text;
                    longevity1 = int.Parse(tbLongevity4.Text);
                    shadeTol1 = int.Parse(tbShadeTol4.Text);
                    maturityAge1 = double.Parse(tbMatAge4.Text);
                    maxANPP1 = int.Parse(tbANPPmax4.Text);
                    maxBiomass = int.Parse(tbBioMax4.Text);
                    if (rbV35.Checked)
                        paramD1 = double.Parse(tbAgeMort4.Text);
                    else
                        paramD1 = double.Parse(tbMortShape4.Text);
                    sppEstab = double.Parse(tbSEC4.Text);
                    leafLongevity = double.Parse(tbLeaf4.Text);
                    seedYear1 = int.Parse(tbSeedYear4.Text);
                    plantYear1 = int.Parse(tbPlant4.Text);
                    removeYear1 = int.Parse(tbRemove4.Text);
                    removeProp1 = double.Parse(tbRemProp4.Text);
                    anppPower1 = double.Parse(tbPower4.Text);
                    decayRate1 = double.Parse(tbDecay4.Text);
                    if (rbV35.Checked)
                        mortMod1 = double.Parse(tbMortMod4.Text);
                    if (cbANPP4.Checked)
                        rangeANPP1 = true;
                    if (cbRangeAP4.Checked)
                        rangeAP1 = true;
                    if (cbRangeAgeMort4.Checked)
                        rangeD1 = true;
                    if (cbRangeMM4.Checked)
                        rangeMM1 = true;
                    resproutProb1 = double.Parse(tbVegProb4.Text);
                    minVegAge1 = int.Parse(tbMinVegAge4.Text);
                    maxVegAge1 = int.Parse(tbMaxVegAge4.Text);
                    budwormHost1 = rbHost4.Checked;
                    budwormDecid1 = rbDecid4.Checked;
                    budwormCondHost1 = rbCondHost4.Checked;
                }
                else if (sppNum5 == 1)
                {
                    sppName = tbSppName5.Text;
                    longevity1 = int.Parse(tbLongevity5.Text);
                    shadeTol1 = int.Parse(tbShadeTol5.Text);
                    maturityAge1 = double.Parse(tbMatAge5.Text);
                    maxANPP1 = int.Parse(tbANPPmax5.Text);
                    maxBiomass = int.Parse(tbBioMax5.Text);
                    if (rbV35.Checked)
                        paramD1 = double.Parse(tbAgeMort5.Text);
                    else
                        paramD1 = double.Parse(tbMortShape5.Text);
                    sppEstab = double.Parse(tbSEC5.Text);
                    leafLongevity = double.Parse(tbLeaf5.Text);
                    seedYear1 = int.Parse(tbSeedYear5.Text);
                    plantYear1 = int.Parse(tbPlant5.Text);
                    removeYear1 = int.Parse(tbRemove5.Text);
                    removeProp1 = double.Parse(tbRemProp5.Text);
                    anppPower1 = double.Parse(tbPower5.Text);
                    decayRate1 = double.Parse(tbDecay5.Text);
                    if (rbV35.Checked)
                        mortMod1 = double.Parse(tbMortMod5.Text);
                    if (cbANPP5.Checked)
                        rangeANPP1 = true;
                    if (cbRangeAP5.Checked)
                        rangeAP1 = true;
                    if (cbRangeAgeMort5.Checked)
                        rangeD1 = true;
                    if (cbRangeMM5.Checked)
                        rangeMM1 = true;
                    resproutProb1 = double.Parse(tbVegProb5.Text);
                    minVegAge1 = int.Parse(tbMinVegAge5.Text);
                    maxVegAge1 = int.Parse(tbMaxVegAge5.Text);
                    budwormHost1 = rbHost5.Checked;
                    budwormDecid1 = rbDecid5.Checked;
                    budwormCondHost1 = rbCondHost5.Checked;
                }
                else if (sppNum6 == 1)
                {
                    sppName = tbSppName6.Text;
                    longevity1 = int.Parse(tbLongevity6.Text);
                    shadeTol1 = int.Parse(tbShadeTol6.Text);
                    maturityAge1 = double.Parse(tbMatAge6.Text);
                    maxANPP1 = int.Parse(tbANPPmax6.Text);
                    maxBiomass = int.Parse(tbBioMax6.Text);
                    if (rbV35.Checked)
                        paramD1 = double.Parse(tbAgeMort6.Text);
                    else
                        paramD1 = double.Parse(tbMortShape6.Text);
                    sppEstab = double.Parse(tbSEC6.Text);
                    leafLongevity = double.Parse(tbLeaf6.Text);
                    seedYear1 = int.Parse(tbSeedYear6.Text);
                    plantYear1 = int.Parse(tbPlant6.Text);
                    removeYear1 = int.Parse(tbRemove6.Text);
                    removeProp1 = double.Parse(tbRemProp6.Text);
                    anppPower1 = double.Parse(tbPower6.Text);
                    decayRate1 = double.Parse(tbDecay6.Text);
                    if (rbV35.Checked)
                        mortMod1 = double.Parse(tbMortMod6.Text);
                    if (cbANPP6.Checked)
                        rangeANPP1 = true;
                    if (cbRangeAP6.Checked)
                        rangeAP1 = true;
                    if (cbRangeAgeMort6.Checked)
                        rangeD1 = true;
                    if (cbRangeMM6.Checked)
                        rangeMM1 = true;
                    resproutProb1 = double.Parse(tbVegProb6.Text);
                    minVegAge1 = int.Parse(tbMinVegAge6.Text);
                    maxVegAge1 = int.Parse(tbMaxVegAge6.Text);
                    budwormHost1 = rbHost6.Checked;
                    budwormDecid1 = rbDecid6.Checked;
                    budwormCondHost1 = rbCondHost6.Checked;
                }
                maxShadeBiomass = maxBiomass;

                X0_1 = Calculations.Compute_X0(longevity1, paramD1);
                mortR_1 = Calculations.Compute_mortR(X0_1, longevity1, paramD1);

                Landis.Species.Parameters parameters1 = new Landis.Species.Parameters(sppName, longevity1, (int)maturityAge1, (byte)shadeTol1, 0, 0, 0, 0, 0, 0, PostFireRegeneration.None);
                Landis.Species.ISpecies species1 = new Landis.Species.Species(0, parameters1);
                speciesList.Add(species1);
                siteMaxBio = maxBiomass;
                //Budworm
                if (budwormHost1) hostSpeciesList.Add(species1);
                else if (budwormCondHost1) condHostSpeciesList.Add(species1);

                if (numSpecies > 1)
                {
                    //Species 2
                    if (sppNum1 == 2)
                    {
                        sppName2 = tbSppName.Text;
                        longevity2 = int.Parse(tbLongevity.Text);
                        shadeTol2 = int.Parse(tbShadeTol.Text);
                        maturityAge2 = double.Parse(tbMatAge1.Text);
                        maxANPP2 = int.Parse(tbANPPmax.Text);
                        maxBiomass2 = int.Parse(tbBioMax.Text);
                        if (rbV35.Checked)
                            paramD2 = double.Parse(tbAgeMort1.Text);
                        else
                            paramD2 = double.Parse(tbMortShape1.Text);
                        sppEstab2 = double.Parse(tbSEC.Text);
                        leafLongevity2 = double.Parse(tbLeaf.Text);
                        seedYear2 = int.Parse(tbSeedYear1.Text);
                        plantYear2 = int.Parse(tbPlant1.Text);
                        removeYear2 = int.Parse(tbRemove1.Text);
                        removeProp2 = double.Parse(tbRemProp1.Text);
                        anppPower2 = double.Parse(tbPower1.Text);
                        decayRate2 = double.Parse(tbDecay1.Text);
                        if (rbV35.Checked)
                        mortMod2 = double.Parse(tbMortMod1.Text);
                        if (cbANPP1.Checked)
                            rangeANPP2 = true;
                        if (cbRangeAP1.Checked)
                            rangeAP2 = true;
                        if (cbRangeAgeMort1.Checked)
                            rangeD2 = true;
                        if (cbRangeMM1.Checked)
                            rangeMM2 = true;
                        resproutProb2 = double.Parse(tbVegProb1.Text);
                        minVegAge2 = int.Parse(tbMinVegAge1.Text);
                        maxVegAge2 = int.Parse(tbMaxVegAge1.Text);
                        budwormHost2 = rbHost1.Checked;
                        budwormDecid2 = rbDecid1.Checked;
                        budwormCondHost2 = rbCondHost1.Checked;
                    }
                    else if (sppNum2 == 2)
                    {
                        sppName2 = tbSppName2.Text;
                        longevity2 = int.Parse(tbLongevity2.Text);
                        shadeTol2 = int.Parse(tbShadeTol2.Text);
                        maturityAge2 = double.Parse(tbMatAge2.Text);
                        maxANPP2 = int.Parse(tbANPPmax2.Text);
                        maxBiomass2 = int.Parse(tbBioMax2.Text);
                        if (rbV35.Checked)
                            paramD2 = double.Parse(tbAgeMort2.Text);
                        else
                            paramD2 = double.Parse(tbMortShape2.Text);
                        sppEstab2 = double.Parse(tbSEC2.Text);
                        leafLongevity2 = double.Parse(tbLeaf2.Text);
                        seedYear2 = int.Parse(tbSeedYear2.Text);
                        plantYear2 = int.Parse(tbPlant2.Text);
                        removeYear2 = int.Parse(tbRemove2.Text);
                        removeProp2 = double.Parse(tbRemProp2.Text);
                        anppPower2 = double.Parse(tbPower2.Text);
                        decayRate2 = double.Parse(tbDecay2.Text);
                        if (rbV35.Checked)
                        mortMod2 = double.Parse(tbMortMod2.Text);
                        if (cbANPP2.Checked)
                            rangeANPP2 = true;
                        if (cbRangeAP2.Checked)
                            rangeAP2 = true;
                        if (cbRangeAgeMort2.Checked)
                            rangeD2 = true;
                        if (cbRangeMM2.Checked)
                            rangeMM2 = true;
                        resproutProb2 = double.Parse(tbVegProb2.Text);
                        minVegAge2 = int.Parse(tbMinVegAge2.Text);
                        maxVegAge2 = int.Parse(tbMaxVegAge2.Text);
                        budwormHost2 = rbHost2.Checked;
                        budwormDecid2 = rbDecid2.Checked;
                        budwormCondHost2 = rbCondHost2.Checked;
                    }
                    else if (sppNum3 == 2)
                    {
                        sppName2 = tbSppName3.Text;
                        longevity2 = int.Parse(tbLongevity3.Text);
                        shadeTol2 = int.Parse(tbShadeTol3.Text);
                        maturityAge2 = double.Parse(tbMatAge3.Text);
                        maxANPP2 = int.Parse(tbANPPmax3.Text);
                        maxBiomass2 = int.Parse(tbBioMax3.Text);
                        if (rbV35.Checked)
                            paramD2 = double.Parse(tbAgeMort3.Text);
                        else
                            paramD2 = double.Parse(tbMortShape3.Text);
                        sppEstab2 = double.Parse(tbSEC3.Text);
                        leafLongevity2 = double.Parse(tbLeaf3.Text);
                        seedYear2 = int.Parse(tbSeedYear3.Text);
                        plantYear2 = int.Parse(tbPlant3.Text);
                        removeYear2 = int.Parse(tbRemove3.Text);
                        removeProp2 = double.Parse(tbRemProp3.Text);
                        anppPower2 = double.Parse(tbPower3.Text);
                        decayRate2 = double.Parse(tbDecay3.Text);
                        if (rbV35.Checked)
                        mortMod2 = double.Parse(tbMortMod3.Text);
                        if (cbANPP3.Checked)
                            rangeANPP2 = true;
                        if (cbRangeAP3.Checked)
                            rangeAP2 = true;
                        if (cbRangeAgeMort3.Checked)
                            rangeD2 = true;
                        if (cbRangeMM3.Checked)
                            rangeMM2 = true;
                        resproutProb2 = double.Parse(tbVegProb3.Text);
                        minVegAge2 = int.Parse(tbMinVegAge3.Text);
                        maxVegAge2 = int.Parse(tbMaxVegAge3.Text);
                        budwormHost2 = rbHost3.Checked;
                        budwormDecid2 = rbDecid3.Checked;
                        budwormCondHost2 = rbCondHost3.Checked;
                    }
                    else if (sppNum4 == 2)
                    {
                        sppName2 = tbSppName4.Text;
                        longevity2 = int.Parse(tbLongevity4.Text);
                        shadeTol2 = int.Parse(tbShadeTol4.Text);
                        maturityAge2 = double.Parse(tbMatAge4.Text);
                        maxANPP2 = int.Parse(tbANPPmax4.Text);
                        maxBiomass2 = int.Parse(tbBioMax4.Text);
                        if (rbV35.Checked)
                            paramD2 = double.Parse(tbAgeMort4.Text);
                        else
                            paramD2 = double.Parse(tbMortShape4.Text);
                        sppEstab2 = double.Parse(tbSEC4.Text);
                        leafLongevity2 = double.Parse(tbLeaf4.Text);
                        seedYear2 = int.Parse(tbSeedYear4.Text);
                        plantYear2 = int.Parse(tbPlant4.Text);
                        removeYear2 = int.Parse(tbRemove4.Text);
                        removeProp2 = double.Parse(tbRemProp4.Text);
                        anppPower2 = double.Parse(tbPower4.Text);
                        decayRate2 = double.Parse(tbDecay4.Text);
                        mortMod2 = double.Parse(tbMortMod4.Text);
                        if (rbV35.Checked)
                        if (cbANPP4.Checked)
                            rangeANPP2 = true;
                        if (cbRangeAP4.Checked)
                            rangeAP2 = true;
                        if (cbRangeAgeMort4.Checked)
                            rangeD2 = true;
                        if (cbRangeMM4.Checked)
                            rangeMM2 = true;
                        resproutProb2 = double.Parse(tbVegProb4.Text);
                        minVegAge2 = int.Parse(tbMinVegAge4.Text);
                        maxVegAge2 = int.Parse(tbMaxVegAge4.Text);
                        budwormHost2 = rbHost4.Checked;
                        budwormDecid2 = rbDecid4.Checked;
                        budwormCondHost2 = rbCondHost4.Checked;
                    }
                    else if (sppNum5 == 2)
                    {
                        sppName2 = tbSppName5.Text;
                        longevity2 = int.Parse(tbLongevity5.Text);
                        shadeTol2 = int.Parse(tbShadeTol5.Text);
                        maturityAge2 = double.Parse(tbMatAge5.Text);
                        maxANPP2 = int.Parse(tbANPPmax5.Text);
                        maxBiomass2 = int.Parse(tbBioMax5.Text);
                        if (rbV35.Checked)
                            paramD2 = double.Parse(tbAgeMort5.Text);
                        else
                            paramD2 = double.Parse(tbMortShape5.Text);
                        sppEstab2 = double.Parse(tbSEC5.Text);
                        leafLongevity2 = double.Parse(tbLeaf5.Text);
                        seedYear2 = int.Parse(tbSeedYear5.Text);
                        plantYear2 = int.Parse(tbPlant5.Text);
                        removeYear2 = int.Parse(tbRemove5.Text);
                        removeProp2 = double.Parse(tbRemProp5.Text);
                        anppPower2 = double.Parse(tbPower5.Text);
                        decayRate2 = double.Parse(tbDecay5.Text);
                        mortMod2 = double.Parse(tbMortMod5.Text);
                        if (rbV35.Checked)
                            if (cbANPP5.Checked)
                                rangeANPP2 = true;
                        if (cbRangeAP5.Checked)
                            rangeAP2 = true;
                        if (cbRangeAgeMort5.Checked)
                            rangeD2 = true;
                        if (cbRangeMM5.Checked)
                            rangeMM2 = true;
                        resproutProb2 = double.Parse(tbVegProb5.Text);
                        minVegAge2 = int.Parse(tbMinVegAge5.Text);
                        maxVegAge2 = int.Parse(tbMaxVegAge5.Text);
                        budwormHost2 = rbHost5.Checked;
                        budwormDecid2 = rbDecid5.Checked;
                        budwormCondHost2 = rbCondHost5.Checked;
                    }
                    else if (sppNum6 == 2)
                    {
                        sppName2 = tbSppName6.Text;
                        longevity2 = int.Parse(tbLongevity6.Text);
                        shadeTol2 = int.Parse(tbShadeTol6.Text);
                        maturityAge2 = double.Parse(tbMatAge6.Text);
                        maxANPP2 = int.Parse(tbANPPmax6.Text);
                        maxBiomass2 = int.Parse(tbBioMax6.Text);
                        if (rbV35.Checked)
                            paramD2 = double.Parse(tbAgeMort6.Text);
                        else
                            paramD2 = double.Parse(tbMortShape6.Text);
                        sppEstab2 = double.Parse(tbSEC6.Text);
                        leafLongevity2 = double.Parse(tbLeaf6.Text);
                        seedYear2 = int.Parse(tbSeedYear6.Text);
                        plantYear2 = int.Parse(tbPlant6.Text);
                        removeYear2 = int.Parse(tbRemove6.Text);
                        removeProp2 = double.Parse(tbRemProp6.Text);
                        anppPower2 = double.Parse(tbPower6.Text);
                        decayRate2 = double.Parse(tbDecay6.Text);
                        mortMod2 = double.Parse(tbMortMod6.Text);
                        if (rbV35.Checked)
                            if (cbANPP6.Checked)
                                rangeANPP2 = true;
                        if (cbRangeAP6.Checked)
                            rangeAP2 = true;
                        if (cbRangeAgeMort6.Checked)
                            rangeD2 = true;
                        if (cbRangeMM6.Checked)
                            rangeMM2 = true;
                        resproutProb2 = double.Parse(tbVegProb6.Text);
                        minVegAge2 = int.Parse(tbMinVegAge6.Text);
                        maxVegAge2 = int.Parse(tbMaxVegAge6.Text);
                        budwormHost2 = rbHost6.Checked;
                        budwormDecid2 = rbDecid6.Checked;
                        budwormCondHost2 = rbCondHost6.Checked;
                    }
                    maxShadeBiomass = Math.Max(maxShadeBiomass, maxBiomass2);

                    X0_2 = Calculations.Compute_X0(longevity2, paramD2);
                    mortR_2 = Calculations.Compute_mortR(X0_2, longevity2, paramD2);

                }
                Landis.Species.Parameters parameters2 = new Landis.Species.Parameters(sppName2, longevity2, (int)maturityAge2, (byte)shadeTol2, 0, 0, 0, 0, 0, 0, PostFireRegeneration.None);
                Landis.Species.Species species2 = new Landis.Species.Species(1, parameters2);

                if (numSpecies > 1)
                {
                    speciesList.Add(species2);
                    siteMaxBio = Math.Max(maxBiomass, maxBiomass2);
                    //Budworm
                    if(budwormHost2) hostSpeciesList.Add(species2);
                    else if (budwormCondHost2) condHostSpeciesList.Add(species2);

                }

                if (numSpecies > 2)
                {
                    //Species 3
                    if (sppNum1 == 3)
                    {
                        sppName3 = tbSppName.Text;
                        longevity3 = int.Parse(tbLongevity.Text);
                        shadeTol3 = int.Parse(tbShadeTol.Text);
                        maturityAge3 = double.Parse(tbMatAge1.Text);
                        maxANPP3 = int.Parse(tbANPPmax.Text);
                        maxBiomass3 = int.Parse(tbBioMax.Text);
                        if (rbV35.Checked)
                            paramD3 = double.Parse(tbAgeMort1.Text);
                        else
                            paramD3 = double.Parse(tbMortShape1.Text);
                        sppEstab3 = double.Parse(tbSEC.Text);
                        leafLongevity3 = double.Parse(tbLeaf.Text);
                        seedYear3 = int.Parse(tbSeedYear1.Text);
                        plantYear3 = int.Parse(tbPlant1.Text);
                        removeYear3 = int.Parse(tbRemove1.Text);
                        removeProp3 = double.Parse(tbRemProp1.Text);
                        anppPower3 = double.Parse(tbPower1.Text);
                        decayRate3 = double.Parse(tbDecay1.Text);
                        if (rbV35.Checked)
                        mortMod3 = double.Parse(tbMortMod1.Text);
                        if (cbANPP1.Checked)
                            rangeANPP3 = true;
                        if (cbRangeAP1.Checked)
                            rangeAP3 = true;
                        if (cbRangeAgeMort1.Checked)
                            rangeD3 = true;
                        if (cbRangeMM1.Checked)
                            rangeMM3 = true;
                        resproutProb3 = double.Parse(tbVegProb1.Text);
                        minVegAge3 = int.Parse(tbMinVegAge1.Text);
                        maxVegAge3 = int.Parse(tbMaxVegAge1.Text);
                        budwormHost3 = rbHost1.Checked;
                        budwormDecid3 = rbDecid1.Checked;
                        budwormCondHost3 = rbCondHost1.Checked;
                    }
                    else if (sppNum2 == 3)
                    {
                        sppName3 = tbSppName2.Text;
                        longevity3 = int.Parse(tbLongevity2.Text);
                        shadeTol3 = int.Parse(tbShadeTol2.Text);
                        maturityAge3 = double.Parse(tbMatAge2.Text);
                        maxANPP3 = int.Parse(tbANPPmax2.Text);
                        maxBiomass3 = int.Parse(tbBioMax2.Text);
                        if (rbV35.Checked)
                            paramD3 = double.Parse(tbAgeMort2.Text);
                        else
                            paramD3 = double.Parse(tbMortShape2.Text);
                        sppEstab3 = double.Parse(tbSEC2.Text);
                        leafLongevity3 = double.Parse(tbLeaf2.Text);
                        seedYear3 = int.Parse(tbSeedYear2.Text);
                        plantYear3 = int.Parse(tbPlant2.Text);
                        removeYear3 = int.Parse(tbRemove2.Text);
                        removeProp3 = double.Parse(tbRemProp2.Text);
                        anppPower3 = double.Parse(tbPower2.Text);
                        decayRate3 = double.Parse(tbDecay2.Text);
                        if (rbV35.Checked)
                        mortMod3 = double.Parse(tbMortMod2.Text);
                        if (cbANPP2.Checked)
                            rangeANPP3 = true;
                        if (cbRangeAP2.Checked)
                            rangeAP3 = true;
                        if (cbRangeAgeMort2.Checked)
                            rangeD3 = true;
                        if (cbRangeMM2.Checked)
                            rangeMM3 = true;
                        resproutProb3 = double.Parse(tbVegProb2.Text);
                        minVegAge3 = int.Parse(tbMinVegAge2.Text);
                        maxVegAge3 = int.Parse(tbMaxVegAge2.Text);
                        budwormHost3 = rbHost2.Checked;
                        budwormDecid3 = rbDecid2.Checked;
                        budwormCondHost3 = rbCondHost2.Checked;
                    }
                    else if (sppNum3 == 3)
                    {
                        sppName3 = tbSppName3.Text;
                        longevity3 = int.Parse(tbLongevity3.Text);
                        shadeTol3 = int.Parse(tbShadeTol3.Text);
                        maturityAge3 = double.Parse(tbMatAge3.Text);
                        maxANPP3 = int.Parse(tbANPPmax3.Text);
                        maxBiomass3 = int.Parse(tbBioMax3.Text);
                        if (rbV35.Checked)
                            paramD3 = double.Parse(tbAgeMort3.Text);
                        else
                            paramD3 = double.Parse(tbMortShape3.Text);
                        sppEstab3 = double.Parse(tbSEC3.Text);
                        leafLongevity3 = double.Parse(tbLeaf3.Text);
                        seedYear3 = int.Parse(tbSeedYear3.Text);
                        plantYear3 = int.Parse(tbPlant3.Text);
                        removeYear3 = int.Parse(tbRemove3.Text);
                        removeProp3 = double.Parse(tbRemProp3.Text);
                        anppPower3 = double.Parse(tbPower3.Text);
                        decayRate3 = double.Parse(tbDecay3.Text);
                        if (rbV35.Checked)
                        mortMod3 = double.Parse(tbMortMod3.Text);
                        if (cbANPP3.Checked)
                            rangeANPP3 = true;
                        if (cbRangeAP3.Checked)
                            rangeAP3 = true;
                        if (cbRangeAgeMort3.Checked)
                            rangeD3 = true;
                        if (cbRangeMM3.Checked)
                            rangeMM3 = true;
                        resproutProb3 = double.Parse(tbVegProb3.Text);
                        minVegAge3 = int.Parse(tbMinVegAge3.Text);
                        maxVegAge3 = int.Parse(tbMaxVegAge3.Text);
                        budwormHost3 = rbHost3.Checked;
                        budwormDecid3 = rbDecid3.Checked;
                        budwormCondHost3 = rbCondHost3.Checked;
                    }
                    else if (sppNum4 == 3)
                    {
                        sppName3 = tbSppName4.Text;
                        longevity3 = int.Parse(tbLongevity4.Text);
                        shadeTol3 = int.Parse(tbShadeTol4.Text);
                        maturityAge3 = double.Parse(tbMatAge4.Text);
                        maxANPP3 = int.Parse(tbANPPmax4.Text);
                        maxBiomass3 = int.Parse(tbBioMax4.Text);
                        if (rbV35.Checked)
                            paramD3 = double.Parse(tbAgeMort4.Text);
                        else
                            paramD3 = double.Parse(tbMortShape4.Text);
                        sppEstab3 = double.Parse(tbSEC4.Text);
                        leafLongevity3 = double.Parse(tbLeaf4.Text);
                        seedYear3 = int.Parse(tbSeedYear4.Text);
                        plantYear3 = int.Parse(tbPlant4.Text);
                        removeYear3 = int.Parse(tbRemove4.Text);
                        removeProp3 = double.Parse(tbRemProp4.Text);
                        anppPower3 = double.Parse(tbPower4.Text);
                        decayRate3 = double.Parse(tbDecay4.Text);
                        if (rbV35.Checked)
                        mortMod3 = double.Parse(tbMortMod4.Text);
                        if (cbANPP4.Checked)
                            rangeANPP3 = true;
                        if (cbRangeAP4.Checked)
                            rangeAP3 = true;
                        if (cbRangeAgeMort4.Checked)
                            rangeD3 = true;
                        if (cbRangeMM4.Checked)
                            rangeMM3 = true;
                        resproutProb3 = double.Parse(tbVegProb4.Text);
                        minVegAge3 = int.Parse(tbMinVegAge4.Text);
                        maxVegAge3 = int.Parse(tbMaxVegAge4.Text);
                        budwormHost3 = rbHost4.Checked;
                        budwormDecid3 = rbDecid4.Checked;
                        budwormCondHost3 = rbCondHost4.Checked;
                    }
                    else if (sppNum5 == 3)
                    {
                        sppName3 = tbSppName5.Text;
                        longevity3 = int.Parse(tbLongevity5.Text);
                        shadeTol3 = int.Parse(tbShadeTol5.Text);
                        maturityAge3 = double.Parse(tbMatAge5.Text);
                        maxANPP3 = int.Parse(tbANPPmax5.Text);
                        maxBiomass3 = int.Parse(tbBioMax5.Text);
                        if (rbV35.Checked)
                            paramD3 = double.Parse(tbAgeMort5.Text);
                        else
                            paramD3 = double.Parse(tbMortShape5.Text);
                        sppEstab3 = double.Parse(tbSEC5.Text);
                        leafLongevity3 = double.Parse(tbLeaf5.Text);
                        seedYear3 = int.Parse(tbSeedYear5.Text);
                        plantYear3 = int.Parse(tbPlant5.Text);
                        removeYear3 = int.Parse(tbRemove5.Text);
                        removeProp3 = double.Parse(tbRemProp5.Text);
                        anppPower3 = double.Parse(tbPower5.Text);
                        decayRate3 = double.Parse(tbDecay5.Text);
                        if (rbV35.Checked)
                            mortMod3 = double.Parse(tbMortMod5.Text);
                        if (cbANPP5.Checked)
                            rangeANPP3 = true;
                        if (cbRangeAP5.Checked)
                            rangeAP3 = true;
                        if (cbRangeAgeMort5.Checked)
                            rangeD3 = true;
                        if (cbRangeMM5.Checked)
                            rangeMM3 = true;
                        resproutProb3 = double.Parse(tbVegProb5.Text);
                        minVegAge3 = int.Parse(tbMinVegAge5.Text);
                        maxVegAge3 = int.Parse(tbMaxVegAge5.Text);
                        budwormHost3 = rbHost5.Checked;
                        budwormDecid3 = rbDecid5.Checked;
                        budwormCondHost3 = rbCondHost5.Checked;
                    }
                    else if (sppNum6 == 3)
                    {
                        sppName3 = tbSppName6.Text;
                        longevity3 = int.Parse(tbLongevity6.Text);
                        shadeTol3 = int.Parse(tbShadeTol6.Text);
                        maturityAge3 = double.Parse(tbMatAge6.Text);
                        maxANPP3 = int.Parse(tbANPPmax6.Text);
                        maxBiomass3 = int.Parse(tbBioMax6.Text);
                        if (rbV35.Checked)
                            paramD3 = double.Parse(tbAgeMort6.Text);
                        else
                            paramD3 = double.Parse(tbMortShape6.Text);
                        sppEstab3 = double.Parse(tbSEC6.Text);
                        leafLongevity3 = double.Parse(tbLeaf6.Text);
                        seedYear3 = int.Parse(tbSeedYear6.Text);
                        plantYear3 = int.Parse(tbPlant6.Text);
                        removeYear3 = int.Parse(tbRemove6.Text);
                        removeProp3 = double.Parse(tbRemProp6.Text);
                        anppPower3 = double.Parse(tbPower6.Text);
                        decayRate3 = double.Parse(tbDecay6.Text);
                        if (rbV35.Checked)
                            mortMod3 = double.Parse(tbMortMod6.Text);
                        if (cbANPP6.Checked)
                            rangeANPP3 = true;
                        if (cbRangeAP6.Checked)
                            rangeAP3 = true;
                        if (cbRangeAgeMort6.Checked)
                            rangeD3 = true;
                        if (cbRangeMM6.Checked)
                            rangeMM3 = true;
                        resproutProb3 = double.Parse(tbVegProb6.Text);
                        minVegAge3 = int.Parse(tbMinVegAge6.Text);
                        maxVegAge3 = int.Parse(tbMaxVegAge6.Text);
                        budwormHost3 = rbHost6.Checked;
                        budwormDecid3 = rbDecid6.Checked;
                        budwormCondHost3 = rbCondHost6.Checked;
                    }
                    maxShadeBiomass = Math.Max(maxShadeBiomass, maxBiomass3);

                    X0_3 = Calculations.Compute_X0(longevity3, paramD3);
                    mortR_3 = Calculations.Compute_mortR(X0_3, longevity3, paramD3);

                }
                Landis.Species.Parameters parameters3 = new Landis.Species.Parameters(sppName3, longevity3, (int)maturityAge3, (byte)shadeTol3, 0, 0, 0, 0, 0, 0, PostFireRegeneration.None);
                Landis.Species.Species species3 = new Landis.Species.Species(2, parameters3);
                
                if (numSpecies > 2)
                {
                    speciesList.Add(species3);
                    siteMaxBio = Math.Max(siteMaxBio, maxBiomass3);
                    //Budworm
                    if(budwormHost3) hostSpeciesList.Add(species3);
                    else if (budwormCondHost3) condHostSpeciesList.Add(species3);

                }

                if (numSpecies > 3)
                {
                    //Species 4
                    if (sppNum1 == 4)
                    {
                        sppName4 = tbSppName.Text;
                        longevity4 = int.Parse(tbLongevity.Text);
                        shadeTol4 = int.Parse(tbShadeTol.Text);
                        maturityAge4 = double.Parse(tbMatAge1.Text);
                        maxANPP4 = int.Parse(tbANPPmax.Text);
                        maxBiomass4 = int.Parse(tbBioMax.Text);
                        if (rbV35.Checked)
                            paramD4 = double.Parse(tbAgeMort1.Text);
                        else
                            paramD4 = double.Parse(tbMortShape1.Text);
                        sppEstab4 = double.Parse(tbSEC.Text);
                        leafLongevity4 = double.Parse(tbLeaf.Text);
                        seedYear4 = int.Parse(tbSeedYear1.Text);
                        plantYear4 = int.Parse(tbPlant1.Text);
                        removeYear4 = int.Parse(tbRemove1.Text);
                        removeProp4 = double.Parse(tbRemProp1.Text);
                        anppPower4 = double.Parse(tbPower1.Text);
                        decayRate4 = double.Parse(tbDecay1.Text);
                        if (rbV35.Checked)
                        mortMod4 = double.Parse(tbMortMod1.Text);
                        if (cbANPP1.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP1.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort1.Checked)
                            rangeD4 = true;
                        if (cbRangeMM1.Checked)
                            rangeMM4 = true;
                        resproutProb4 = double.Parse(tbVegProb1.Text);
                        minVegAge4 = int.Parse(tbMinVegAge1.Text);
                        maxVegAge4 = int.Parse(tbMaxVegAge1.Text);
                        budwormHost4 = rbHost1.Checked;
                        budwormDecid4 = rbDecid1.Checked;
                        budwormCondHost4 = rbCondHost1.Checked;
                    }
                    else if (sppNum2 == 4)
                    {
                        sppName4 = tbSppName2.Text;
                        longevity4 = int.Parse(tbLongevity2.Text);
                        shadeTol4 = int.Parse(tbShadeTol2.Text);
                        maturityAge4 = double.Parse(tbMatAge2.Text);
                        maxANPP4 = int.Parse(tbANPPmax2.Text);
                        maxBiomass4 = int.Parse(tbBioMax2.Text);
                        if (rbV35.Checked)
                            paramD4 = double.Parse(tbAgeMort2.Text);
                        else
                            paramD4 = double.Parse(tbMortShape2.Text);
                        sppEstab4 = double.Parse(tbSEC2.Text);
                        leafLongevity4 = double.Parse(tbLeaf2.Text);
                        seedYear4 = int.Parse(tbSeedYear2.Text);
                        plantYear4 = int.Parse(tbPlant2.Text);
                        removeYear4 = int.Parse(tbRemove2.Text);
                        removeProp4 = double.Parse(tbRemProp2.Text);
                        anppPower4 = double.Parse(tbPower2.Text);
                        decayRate4 = double.Parse(tbDecay2.Text);
                        if (rbV35.Checked)
                        mortMod4 = double.Parse(tbMortMod2.Text);
                        if (cbANPP2.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP2.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort2.Checked)
                            rangeD4 = true;
                        if (cbRangeMM2.Checked)
                            rangeMM4 = true;
                        resproutProb4 = double.Parse(tbVegProb2.Text);
                        minVegAge4 = int.Parse(tbMinVegAge2.Text);
                        maxVegAge4 = int.Parse(tbMaxVegAge2.Text);
                        budwormHost4 = rbHost2.Checked;
                        budwormDecid4 = rbDecid2.Checked;
                        budwormCondHost4 = rbCondHost2.Checked;
                    }
                    else if (sppNum3 == 4)
                    {
                        sppName4 = tbSppName3.Text;
                        longevity4 = int.Parse(tbLongevity3.Text);
                        shadeTol4 = int.Parse(tbShadeTol3.Text);
                        maturityAge4 = double.Parse(tbMatAge3.Text);
                        maxANPP4 = int.Parse(tbANPPmax3.Text);
                        maxBiomass4 = int.Parse(tbBioMax3.Text);
                        if (rbV35.Checked)
                            paramD4 = double.Parse(tbAgeMort3.Text);
                        else
                            paramD4 = double.Parse(tbMortShape3.Text);
                        sppEstab4 = double.Parse(tbSEC3.Text);
                        leafLongevity4 = double.Parse(tbLeaf3.Text);
                        seedYear4 = int.Parse(tbSeedYear3.Text);
                        plantYear4 = int.Parse(tbPlant3.Text);
                        removeYear4 = int.Parse(tbRemove3.Text);
                        removeProp4 = double.Parse(tbRemProp3.Text);
                        anppPower4 = double.Parse(tbPower3.Text);
                        decayRate4 = double.Parse(tbDecay3.Text);
                        if (rbV35.Checked)
                        mortMod4 = double.Parse(tbMortMod3.Text);
                        if (cbANPP3.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP3.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort3.Checked)
                            rangeD4 = true;
                        if (cbRangeMM3.Checked)
                            rangeMM4 = true;
                        resproutProb4 = double.Parse(tbVegProb3.Text);
                        minVegAge4 = int.Parse(tbMinVegAge3.Text);
                        maxVegAge4 = int.Parse(tbMaxVegAge3.Text);
                        budwormHost4 = rbHost3.Checked;
                        budwormDecid4 = rbDecid3.Checked;
                        budwormCondHost4 = rbCondHost3.Checked;
                    }
                    else if (sppNum4 == 4)
                    {
                        sppName4 = tbSppName4.Text;
                        longevity4 = int.Parse(tbLongevity4.Text);
                        shadeTol4 = int.Parse(tbShadeTol4.Text);
                        maturityAge4 = double.Parse(tbMatAge4.Text);
                        maxANPP4 = int.Parse(tbANPPmax4.Text);
                        maxBiomass4 = int.Parse(tbBioMax4.Text);
                        if (rbV35.Checked)
                            paramD4 = double.Parse(tbAgeMort4.Text);
                        else
                            paramD4 = double.Parse(tbMortShape4.Text);
                        sppEstab4 = double.Parse(tbSEC4.Text);
                        leafLongevity4 = double.Parse(tbLeaf4.Text);
                        seedYear4 = int.Parse(tbSeedYear4.Text);
                        plantYear4 = int.Parse(tbPlant4.Text);
                        removeYear4 = int.Parse(tbRemove4.Text);
                        removeProp4 = double.Parse(tbRemProp4.Text);
                        anppPower4 = double.Parse(tbPower4.Text);
                        decayRate4 = double.Parse(tbDecay4.Text);
                        if (rbV35.Checked)
                        mortMod4 = double.Parse(tbMortMod4.Text);
                        if (cbANPP4.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP4.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort4.Checked)
                            rangeD4 = true;
                        if (cbRangeMM4.Checked)
                            rangeMM4 = true;
                        resproutProb4 = double.Parse(tbVegProb4.Text);
                        minVegAge4 = int.Parse(tbMinVegAge4.Text);
                        maxVegAge4 = int.Parse(tbMaxVegAge4.Text);
                        budwormHost4 = rbHost4.Checked;
                        budwormDecid4 = rbDecid4.Checked;
                        budwormCondHost4 = rbCondHost4.Checked;
                    }
                    else if (sppNum5 == 4)
                    {
                        sppName4 = tbSppName5.Text;
                        longevity4 = int.Parse(tbLongevity5.Text);
                        shadeTol4 = int.Parse(tbShadeTol5.Text);
                        maturityAge4 = double.Parse(tbMatAge5.Text);
                        maxANPP4 = int.Parse(tbANPPmax5.Text);
                        maxBiomass4 = int.Parse(tbBioMax5.Text);
                        if (rbV35.Checked)
                            paramD4 = double.Parse(tbAgeMort5.Text);
                        else
                            paramD4 = double.Parse(tbMortShape5.Text);
                        sppEstab4 = double.Parse(tbSEC5.Text);
                        leafLongevity4 = double.Parse(tbLeaf5.Text);
                        seedYear4 = int.Parse(tbSeedYear5.Text);
                        plantYear4 = int.Parse(tbPlant5.Text);
                        removeYear4 = int.Parse(tbRemove5.Text);
                        removeProp4 = double.Parse(tbRemProp5.Text);
                        anppPower4 = double.Parse(tbPower5.Text);
                        decayRate4 = double.Parse(tbDecay5.Text);
                        if (rbV35.Checked)
                            mortMod4 = double.Parse(tbMortMod5.Text);
                        if (cbANPP5.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP5.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort5.Checked)
                            rangeD4 = true;
                        if (cbRangeMM5.Checked)
                            rangeMM4 = true;
                        resproutProb4 = double.Parse(tbVegProb5.Text);
                        minVegAge4 = int.Parse(tbMinVegAge5.Text);
                        maxVegAge4 = int.Parse(tbMaxVegAge5.Text);
                        budwormHost4 = rbHost5.Checked;
                        budwormDecid4 = rbDecid5.Checked;
                        budwormCondHost4 = rbCondHost5.Checked;
                    }
                    else if (sppNum6 == 4)
                    {
                        sppName4 = tbSppName6.Text;
                        longevity4 = int.Parse(tbLongevity6.Text);
                        shadeTol4 = int.Parse(tbShadeTol6.Text);
                        maturityAge4 = double.Parse(tbMatAge6.Text);
                        maxANPP4 = int.Parse(tbANPPmax6.Text);
                        maxBiomass4 = int.Parse(tbBioMax6.Text);
                        if (rbV35.Checked)
                            paramD4 = double.Parse(tbAgeMort6.Text);
                        else
                            paramD4 = double.Parse(tbMortShape6.Text);
                        sppEstab4 = double.Parse(tbSEC6.Text);
                        leafLongevity4 = double.Parse(tbLeaf6.Text);
                        seedYear4 = int.Parse(tbSeedYear6.Text);
                        plantYear4 = int.Parse(tbPlant6.Text);
                        removeYear4 = int.Parse(tbRemove6.Text);
                        removeProp4 = double.Parse(tbRemProp6.Text);
                        anppPower4 = double.Parse(tbPower6.Text);
                        decayRate4 = double.Parse(tbDecay6.Text);
                        if (rbV35.Checked)
                            mortMod4 = double.Parse(tbMortMod6.Text);
                        if (cbANPP6.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP6.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort6.Checked)
                            rangeD4 = true;
                        if (cbRangeMM6.Checked)
                            rangeMM4 = true;
                        resproutProb4 = double.Parse(tbVegProb6.Text);
                        minVegAge4 = int.Parse(tbMinVegAge6.Text);
                        maxVegAge4 = int.Parse(tbMaxVegAge6.Text);
                        budwormHost4 = rbHost6.Checked;
                        budwormDecid4 = rbDecid6.Checked;
                        budwormCondHost4 = rbCondHost6.Checked;
                    }
                    maxShadeBiomass = Math.Max(maxShadeBiomass, maxBiomass4);
                    

                    X0_4 = Calculations.Compute_X0(longevity4, paramD4);
                    mortR_4 = Calculations.Compute_mortR(X0_4, longevity4, paramD4);

                }

                maxShadeBiomass = Math.Max(maxShadeBiomass, landMaxBio);
                Landis.Species.Parameters parameters4 = new Landis.Species.Parameters(sppName4, longevity4, (int)maturityAge4, (byte)shadeTol4, 0, 0, 0, 0, 0, 0, PostFireRegeneration.None);
                Landis.Species.Species species4 = new Landis.Species.Species(3, parameters4);

                if (numSpecies > 3)
                {
                    speciesList.Add(species4);
                    siteMaxBio = Math.Max(siteMaxBio, maxBiomass4);
                    //Budworm
                    if(budwormHost4) hostSpeciesList.Add(species4);
                    else if (budwormCondHost4) condHostSpeciesList.Add(species4);

                }

                if (numSpecies > 4)
                {
                    //Species 5
                    if (sppNum1 == 5)
                    {
                        sppName5 = tbSppName.Text;
                        longevity5 = int.Parse(tbLongevity.Text);
                        shadeTol5 = int.Parse(tbShadeTol.Text);
                        maturityAge5 = double.Parse(tbMatAge1.Text);
                        maxANPP5 = int.Parse(tbANPPmax.Text);
                        maxBiomass5 = int.Parse(tbBioMax.Text);
                        if (rbV35.Checked)
                            paramD5 = double.Parse(tbAgeMort1.Text);
                        else
                            paramD5 = double.Parse(tbMortShape1.Text);
                        sppEstab5 = double.Parse(tbSEC.Text);
                        leafLongevity5 = double.Parse(tbLeaf.Text);
                        seedYear5 = int.Parse(tbSeedYear1.Text);
                        plantYear5 = int.Parse(tbPlant1.Text);
                        removeYear5 = int.Parse(tbRemove1.Text);
                        removeProp5 = double.Parse(tbRemProp1.Text);
                        anppPower5 = double.Parse(tbPower1.Text);
                        decayRate5 = double.Parse(tbDecay1.Text);
                        if (rbV35.Checked)
                            mortMod5 = double.Parse(tbMortMod1.Text);
                        if (cbANPP1.Checked)
                            rangeANPP5 = true;
                        if (cbRangeAP1.Checked)
                            rangeAP5 = true;
                        if (cbRangeAgeMort1.Checked)
                            rangeD5 = true;
                        if (cbRangeMM1.Checked)
                            rangeMM5 = true;
                        resproutProb5 = double.Parse(tbVegProb1.Text);
                        minVegAge5 = int.Parse(tbMinVegAge1.Text);
                        maxVegAge5 = int.Parse(tbMaxVegAge1.Text);
                        budwormHost5 = rbHost1.Checked;
                        budwormDecid5 = rbDecid1.Checked;
                        budwormCondHost5 = rbCondHost1.Checked;
                    }
                    else if (sppNum2 == 5)
                    {
                        sppName5 = tbSppName2.Text;
                        longevity5 = int.Parse(tbLongevity2.Text);
                        shadeTol5 = int.Parse(tbShadeTol2.Text);
                        maturityAge5 = double.Parse(tbMatAge2.Text);
                        maxANPP5 = int.Parse(tbANPPmax2.Text);
                        maxBiomass5 = int.Parse(tbBioMax2.Text);
                        if (rbV35.Checked)
                            paramD5 = double.Parse(tbAgeMort2.Text);
                        else
                            paramD5 = double.Parse(tbMortShape2.Text);
                        sppEstab5 = double.Parse(tbSEC2.Text);
                        leafLongevity5 = double.Parse(tbLeaf2.Text);
                        seedYear5 = int.Parse(tbSeedYear2.Text);
                        plantYear5 = int.Parse(tbPlant2.Text);
                        removeYear5 = int.Parse(tbRemove2.Text);
                        removeProp5 = double.Parse(tbRemProp2.Text);
                        anppPower5 = double.Parse(tbPower2.Text);
                        decayRate5 = double.Parse(tbDecay2.Text);
                        if (rbV35.Checked)
                            mortMod5 = double.Parse(tbMortMod2.Text);
                        if (cbANPP2.Checked)
                            rangeANPP5 = true;
                        if (cbRangeAP2.Checked)
                            rangeAP5 = true;
                        if (cbRangeAgeMort2.Checked)
                            rangeD5 = true;
                        if (cbRangeMM2.Checked)
                            rangeMM5 = true;
                        resproutProb5 = double.Parse(tbVegProb2.Text);
                        minVegAge5 = int.Parse(tbMinVegAge2.Text);
                        maxVegAge5 = int.Parse(tbMaxVegAge2.Text);
                        budwormHost5 = rbHost2.Checked;
                        budwormDecid5 = rbDecid2.Checked;
                        budwormCondHost5 = rbCondHost2.Checked;
                    }
                    else if (sppNum3 == 5)
                    {
                        sppName5 = tbSppName3.Text;
                        longevity5 = int.Parse(tbLongevity3.Text);
                        shadeTol5 = int.Parse(tbShadeTol3.Text);
                        maturityAge5 = double.Parse(tbMatAge3.Text);
                        maxANPP5 = int.Parse(tbANPPmax3.Text);
                        maxBiomass5 = int.Parse(tbBioMax3.Text);
                        if (rbV35.Checked)
                            paramD5 = double.Parse(tbAgeMort3.Text);
                        else
                            paramD5 = double.Parse(tbMortShape3.Text);
                        sppEstab5 = double.Parse(tbSEC3.Text);
                        leafLongevity5 = double.Parse(tbLeaf3.Text);
                        seedYear5 = int.Parse(tbSeedYear3.Text);
                        plantYear5 = int.Parse(tbPlant3.Text);
                        removeYear5 = int.Parse(tbRemove3.Text);
                        removeProp5 = double.Parse(tbRemProp3.Text);
                        anppPower5 = double.Parse(tbPower3.Text);
                        decayRate5 = double.Parse(tbDecay3.Text);
                        if (rbV35.Checked)
                            mortMod5 = double.Parse(tbMortMod3.Text);
                        if (cbANPP3.Checked)
                            rangeANPP5 = true;
                        if (cbRangeAP3.Checked)
                            rangeAP5 = true;
                        if (cbRangeAgeMort3.Checked)
                            rangeD5 = true;
                        if (cbRangeMM3.Checked)
                            rangeMM5 = true;
                        resproutProb5 = double.Parse(tbVegProb3.Text);
                        minVegAge5 = int.Parse(tbMinVegAge3.Text);
                        maxVegAge5 = int.Parse(tbMaxVegAge3.Text);
                        budwormHost5 = rbHost3.Checked;
                        budwormDecid5 = rbDecid3.Checked;
                        budwormCondHost5 = rbCondHost3.Checked;
                    }
                    else if (sppNum4 == 5)
                    {
                        sppName5 = tbSppName4.Text;
                        longevity5 = int.Parse(tbLongevity4.Text);
                        shadeTol5 = int.Parse(tbShadeTol4.Text);
                        maturityAge5 = double.Parse(tbMatAge4.Text);
                        maxANPP5 = int.Parse(tbANPPmax4.Text);
                        maxBiomass5 = int.Parse(tbBioMax4.Text);
                        if (rbV35.Checked)
                            paramD5 = double.Parse(tbAgeMort4.Text);
                        else
                            paramD5 = double.Parse(tbMortShape4.Text);
                        sppEstab5 = double.Parse(tbSEC4.Text);
                        leafLongevity5 = double.Parse(tbLeaf4.Text);
                        seedYear5 = int.Parse(tbSeedYear4.Text);
                        plantYear5 = int.Parse(tbPlant4.Text);
                        removeYear5 = int.Parse(tbRemove4.Text);
                        removeProp5 = double.Parse(tbRemProp4.Text);
                        anppPower5 = double.Parse(tbPower4.Text);
                        decayRate5 = double.Parse(tbDecay4.Text);
                        if (rbV35.Checked)
                            mortMod5 = double.Parse(tbMortMod4.Text);
                        if (cbANPP5.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP5.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort5.Checked)
                            rangeD4 = true;
                        if (cbRangeMM5.Checked)
                            rangeMM4 = true;
                        resproutProb5 = double.Parse(tbVegProb4.Text);
                        minVegAge5 = int.Parse(tbMinVegAge4.Text);
                        maxVegAge5 = int.Parse(tbMaxVegAge4.Text);
                        budwormHost5 = rbHost4.Checked;
                        budwormDecid5 = rbDecid4.Checked;
                        budwormCondHost5 = rbCondHost4.Checked;
                    }
                    else if (sppNum5 == 5)
                    {
                        sppName5 = tbSppName5.Text;
                        longevity5 = int.Parse(tbLongevity5.Text);
                        shadeTol5 = int.Parse(tbShadeTol5.Text);
                        maturityAge5 = double.Parse(tbMatAge5.Text);
                        maxANPP5 = int.Parse(tbANPPmax5.Text);
                        maxBiomass5 = int.Parse(tbBioMax5.Text);
                        if (rbV35.Checked)
                            paramD5 = double.Parse(tbAgeMort5.Text);
                        else
                            paramD5 = double.Parse(tbMortShape5.Text);
                        sppEstab5 = double.Parse(tbSEC5.Text);
                        leafLongevity5 = double.Parse(tbLeaf5.Text);
                        seedYear5 = int.Parse(tbSeedYear5.Text);
                        plantYear5 = int.Parse(tbPlant5.Text);
                        removeYear5 = int.Parse(tbRemove5.Text);
                        removeProp5 = double.Parse(tbRemProp5.Text);
                        anppPower5 = double.Parse(tbPower5.Text);
                        decayRate5 = double.Parse(tbDecay5.Text);
                        if (rbV35.Checked)
                            mortMod5 = double.Parse(tbMortMod5.Text);
                        if (cbANPP5.Checked)
                            rangeANPP5 = true;
                        if (cbRangeAP5.Checked)
                            rangeAP5 = true;
                        if (cbRangeAgeMort5.Checked)
                            rangeD5 = true;
                        if (cbRangeMM5.Checked)
                            rangeMM5 = true;
                        resproutProb5 = double.Parse(tbVegProb5.Text);
                        minVegAge5 = int.Parse(tbMinVegAge5.Text);
                        maxVegAge5 = int.Parse(tbMaxVegAge5.Text);
                        budwormHost5 = rbHost5.Checked;
                        budwormDecid5 = rbDecid5.Checked;
                        budwormCondHost5 = rbCondHost5.Checked;
                    }
                    else if (sppNum6 == 5)
                    {
                        sppName5 = tbSppName6.Text;
                        longevity5 = int.Parse(tbLongevity6.Text);
                        shadeTol5 = int.Parse(tbShadeTol6.Text);
                        maturityAge5 = double.Parse(tbMatAge6.Text);
                        maxANPP5 = int.Parse(tbANPPmax6.Text);
                        maxBiomass5 = int.Parse(tbBioMax6.Text);
                        if (rbV35.Checked)
                            paramD5 = double.Parse(tbAgeMort6.Text);
                        else
                            paramD5 = double.Parse(tbMortShape6.Text);
                        sppEstab5 = double.Parse(tbSEC6.Text);
                        leafLongevity5 = double.Parse(tbLeaf6.Text);
                        seedYear5 = int.Parse(tbSeedYear6.Text);
                        plantYear5 = int.Parse(tbPlant6.Text);
                        removeYear5 = int.Parse(tbRemove6.Text);
                        removeProp5 = double.Parse(tbRemProp6.Text);
                        anppPower5 = double.Parse(tbPower6.Text);
                        decayRate5 = double.Parse(tbDecay6.Text);
                        if (rbV35.Checked)
                            mortMod5 = double.Parse(tbMortMod6.Text);
                        if (cbANPP6.Checked)
                            rangeANPP5 = true;
                        if (cbRangeAP6.Checked)
                            rangeAP5 = true;
                        if (cbRangeAgeMort6.Checked)
                            rangeD5 = true;
                        if (cbRangeMM6.Checked)
                            rangeMM5 = true;
                        resproutProb5 = double.Parse(tbVegProb6.Text);
                        minVegAge5 = int.Parse(tbMinVegAge6.Text);
                        maxVegAge5 = int.Parse(tbMaxVegAge6.Text);
                        budwormHost5 = rbHost6.Checked;
                        budwormDecid5 = rbDecid6.Checked;
                        budwormCondHost5 = rbCondHost6.Checked;
                    }
                    maxShadeBiomass = Math.Max(maxShadeBiomass, maxBiomass5);


                    X0_5 = Calculations.Compute_X0(longevity5, paramD5);
                    mortR_5 = Calculations.Compute_mortR(X0_5, longevity5, paramD5);

                }

                maxShadeBiomass = Math.Max(maxShadeBiomass, landMaxBio);
                Landis.Species.Parameters parameters5 = new Landis.Species.Parameters(sppName5, longevity5, (int)maturityAge5, (byte)shadeTol5, 0, 0, 0, 0, 0, 0, PostFireRegeneration.None);
                Landis.Species.Species species5 = new Landis.Species.Species(3, parameters5);

                if (numSpecies > 4)
                {
                    speciesList.Add(species5);
                    siteMaxBio = Math.Max(siteMaxBio, maxBiomass5);
                    //Budworm
                    if(budwormHost5) hostSpeciesList.Add(species5);
                    else if (budwormCondHost5) condHostSpeciesList.Add(species5);

                }
                if (numSpecies > 5)
                {
                    //Species 6
                    if (sppNum1 == 6)
                    {
                        sppName6 = tbSppName.Text;
                        longevity6 = int.Parse(tbLongevity.Text);
                        shadeTol6 = int.Parse(tbShadeTol.Text);
                        maturityAge6 = double.Parse(tbMatAge1.Text);
                        maxANPP6 = int.Parse(tbANPPmax.Text);
                        maxBiomass6 = int.Parse(tbBioMax.Text);
                        if (rbV35.Checked)
                            paramD6 = double.Parse(tbAgeMort1.Text);
                        else
                            paramD6 = double.Parse(tbMortShape1.Text);
                        sppEstab6 = double.Parse(tbSEC.Text);
                        leafLongevity6 = double.Parse(tbLeaf.Text);
                        seedYear6 = int.Parse(tbSeedYear1.Text);
                        plantYear6 = int.Parse(tbPlant1.Text);
                        removeYear6 = int.Parse(tbRemove1.Text);
                        removeProp6 = double.Parse(tbRemProp1.Text);
                        anppPower6 = double.Parse(tbPower1.Text);
                        decayRate6 = double.Parse(tbDecay1.Text);
                        if (rbV35.Checked)
                            mortMod6 = double.Parse(tbMortMod1.Text);
                        if (cbANPP1.Checked)
                            rangeANPP6 = true;
                        if (cbRangeAP1.Checked)
                            rangeAP6 = true;
                        if (cbRangeAgeMort1.Checked)
                            rangeD6 = true;
                        if (cbRangeMM1.Checked)
                            rangeMM6 = true;
                        resproutProb6 = double.Parse(tbVegProb1.Text);
                        minVegAge6 = int.Parse(tbMinVegAge1.Text);
                        maxVegAge6 = int.Parse(tbMaxVegAge1.Text);
                        budwormHost6 = rbHost1.Checked;
                        budwormDecid6 = rbDecid1.Checked;
                        budwormCondHost6 = rbCondHost1.Checked;
                    }
                    else if (sppNum2 == 6)
                    {
                        sppName6 = tbSppName2.Text;
                        longevity6 = int.Parse(tbLongevity2.Text);
                        shadeTol6 = int.Parse(tbShadeTol2.Text);
                        maturityAge6 = double.Parse(tbMatAge2.Text);
                        maxANPP6 = int.Parse(tbANPPmax2.Text);
                        maxBiomass6 = int.Parse(tbBioMax2.Text);
                        if (rbV35.Checked)
                            paramD6 = double.Parse(tbAgeMort2.Text);
                        else
                            paramD6 = double.Parse(tbMortShape2.Text);
                        sppEstab6 = double.Parse(tbSEC2.Text);
                        leafLongevity6 = double.Parse(tbLeaf2.Text);
                        seedYear6 = int.Parse(tbSeedYear2.Text);
                        plantYear6 = int.Parse(tbPlant2.Text);
                        removeYear6 = int.Parse(tbRemove2.Text);
                        removeProp6 = double.Parse(tbRemProp2.Text);
                        anppPower6 = double.Parse(tbPower2.Text);
                        decayRate6 = double.Parse(tbDecay2.Text);
                        if (rbV35.Checked)
                            mortMod6 = double.Parse(tbMortMod2.Text);
                        if (cbANPP2.Checked)
                            rangeANPP6 = true;
                        if (cbRangeAP2.Checked)
                            rangeAP6 = true;
                        if (cbRangeAgeMort2.Checked)
                            rangeD6 = true;
                        if (cbRangeMM2.Checked)
                            rangeMM6 = true;
                        resproutProb6 = double.Parse(tbVegProb2.Text);
                        minVegAge6 = int.Parse(tbMinVegAge2.Text);
                        maxVegAge6 = int.Parse(tbMaxVegAge2.Text);
                        budwormHost6 = rbHost2.Checked;
                        budwormDecid6 = rbDecid2.Checked;
                        budwormCondHost6 = rbCondHost2.Checked;
                    }
                    else if (sppNum3 == 6)
                    {
                        sppName6 = tbSppName3.Text;
                        longevity6 = int.Parse(tbLongevity3.Text);
                        shadeTol6 = int.Parse(tbShadeTol3.Text);
                        maturityAge6 = double.Parse(tbMatAge3.Text);
                        maxANPP6 = int.Parse(tbANPPmax3.Text);
                        maxBiomass6 = int.Parse(tbBioMax3.Text);
                        if (rbV35.Checked)
                            paramD6 = double.Parse(tbAgeMort3.Text);
                        else
                            paramD6 = double.Parse(tbMortShape3.Text);
                        sppEstab6 = double.Parse(tbSEC3.Text);
                        leafLongevity6 = double.Parse(tbLeaf3.Text);
                        seedYear6 = int.Parse(tbSeedYear3.Text);
                        plantYear6 = int.Parse(tbPlant3.Text);
                        removeYear6 = int.Parse(tbRemove3.Text);
                        removeProp6 = double.Parse(tbRemProp3.Text);
                        anppPower6 = double.Parse(tbPower3.Text);
                        decayRate6 = double.Parse(tbDecay3.Text);
                        if (rbV35.Checked)
                            mortMod6 = double.Parse(tbMortMod3.Text);
                        if (cbANPP3.Checked)
                            rangeANPP6 = true;
                        if (cbRangeAP3.Checked)
                            rangeAP6 = true;
                        if (cbRangeAgeMort3.Checked)
                            rangeD6 = true;
                        if (cbRangeMM3.Checked)
                            rangeMM6 = true;
                        resproutProb6 = double.Parse(tbVegProb3.Text);
                        minVegAge6 = int.Parse(tbMinVegAge3.Text);
                        maxVegAge6 = int.Parse(tbMaxVegAge3.Text);
                        budwormHost6 = rbHost3.Checked;
                        budwormDecid6 = rbDecid3.Checked;
                        budwormCondHost6 = rbCondHost3.Checked;
                    }
                    else if (sppNum4 == 6)
                    {
                        sppName6 = tbSppName4.Text;
                        longevity6 = int.Parse(tbLongevity4.Text);
                        shadeTol6 = int.Parse(tbShadeTol4.Text);
                        maturityAge6 = double.Parse(tbMatAge4.Text);
                        maxANPP6 = int.Parse(tbANPPmax4.Text);
                        maxBiomass6 = int.Parse(tbBioMax4.Text);
                        if (rbV35.Checked)
                            paramD6 = double.Parse(tbAgeMort4.Text);
                        else
                            paramD6 = double.Parse(tbMortShape4.Text);
                        sppEstab6 = double.Parse(tbSEC4.Text);
                        leafLongevity6 = double.Parse(tbLeaf4.Text);
                        seedYear6 = int.Parse(tbSeedYear4.Text);
                        plantYear6 = int.Parse(tbPlant4.Text);
                        removeYear6 = int.Parse(tbRemove4.Text);
                        removeProp6 = double.Parse(tbRemProp4.Text);
                        anppPower6 = double.Parse(tbPower4.Text);
                        decayRate6 = double.Parse(tbDecay4.Text);
                        if (rbV35.Checked)
                            mortMod6 = double.Parse(tbMortMod4.Text);
                        if (cbANPP6.Checked)
                            rangeANPP4 = true;
                        if (cbRangeAP6.Checked)
                            rangeAP4 = true;
                        if (cbRangeAgeMort6.Checked)
                            rangeD4 = true;
                        if (cbRangeMM6.Checked)
                            rangeMM4 = true;
                        resproutProb6 = double.Parse(tbVegProb4.Text);
                        minVegAge6 = int.Parse(tbMinVegAge4.Text);
                        maxVegAge6 = int.Parse(tbMaxVegAge4.Text);
                        budwormHost6 = rbHost4.Checked;
                        budwormDecid6 = rbDecid4.Checked;
                        budwormCondHost6 = rbCondHost4.Checked;
                    }
                    else if (sppNum5 == 6)
                    {
                        sppName6 = tbSppName5.Text;
                        longevity6 = int.Parse(tbLongevity5.Text);
                        shadeTol6 = int.Parse(tbShadeTol5.Text);
                        maturityAge6 = double.Parse(tbMatAge5.Text);
                        maxANPP6 = int.Parse(tbANPPmax5.Text);
                        maxBiomass6 = int.Parse(tbBioMax5.Text);
                        if (rbV35.Checked)
                            paramD6 = double.Parse(tbAgeMort5.Text);
                        else
                            paramD6 = double.Parse(tbMortShape5.Text);
                        sppEstab6 = double.Parse(tbSEC5.Text);
                        leafLongevity6 = double.Parse(tbLeaf5.Text);
                        seedYear6 = int.Parse(tbSeedYear5.Text);
                        plantYear6 = int.Parse(tbPlant5.Text);
                        removeYear6 = int.Parse(tbRemove5.Text);
                        removeProp6 = double.Parse(tbRemProp5.Text);
                        anppPower6 = double.Parse(tbPower5.Text);
                        decayRate6 = double.Parse(tbDecay5.Text);
                        if (rbV35.Checked)
                            mortMod6 = double.Parse(tbMortMod5.Text);
                        if (cbANPP5.Checked)
                            rangeANPP6 = true;
                        if (cbRangeAP5.Checked)
                            rangeAP6 = true;
                        if (cbRangeAgeMort5.Checked)
                            rangeD6 = true;
                        if (cbRangeMM5.Checked)
                            rangeMM6 = true;
                        resproutProb6 = double.Parse(tbVegProb5.Text);
                        minVegAge6 = int.Parse(tbMinVegAge5.Text);
                        maxVegAge6 = int.Parse(tbMaxVegAge5.Text);
                        budwormHost6 = rbHost5.Checked;
                        budwormDecid6 = rbDecid5.Checked;
                        budwormCondHost6 = rbCondHost5.Checked;
                    }
                    else if (sppNum6 == 6)
                    {
                        sppName6 = tbSppName6.Text;
                        longevity6 = int.Parse(tbLongevity6.Text);
                        shadeTol6 = int.Parse(tbShadeTol6.Text);
                        maturityAge6 = double.Parse(tbMatAge6.Text);
                        maxANPP6 = int.Parse(tbANPPmax6.Text);
                        maxBiomass6 = int.Parse(tbBioMax6.Text);
                        if (rbV35.Checked)
                            paramD6 = double.Parse(tbAgeMort6.Text);
                        else
                            paramD6 = double.Parse(tbMortShape6.Text);
                        sppEstab6 = double.Parse(tbSEC6.Text);
                        leafLongevity6 = double.Parse(tbLeaf6.Text);
                        seedYear6 = int.Parse(tbSeedYear6.Text);
                        plantYear6 = int.Parse(tbPlant6.Text);
                        removeYear6 = int.Parse(tbRemove6.Text);
                        removeProp6 = double.Parse(tbRemProp6.Text);
                        anppPower6 = double.Parse(tbPower6.Text);
                        decayRate6 = double.Parse(tbDecay6.Text);
                        if (rbV35.Checked)
                            mortMod6 = double.Parse(tbMortMod6.Text);
                        if (cbANPP6.Checked)
                            rangeANPP6 = true;
                        if (cbRangeAP6.Checked)
                            rangeAP6 = true;
                        if (cbRangeAgeMort6.Checked)
                            rangeD6 = true;
                        if (cbRangeMM6.Checked)
                            rangeMM6 = true;
                        resproutProb6 = double.Parse(tbVegProb6.Text);
                        minVegAge6 = int.Parse(tbMinVegAge6.Text);
                        maxVegAge6 = int.Parse(tbMaxVegAge6.Text);
                        budwormHost6 = rbHost6.Checked;
                        budwormDecid6 = rbDecid6.Checked;
                        budwormCondHost6 = rbCondHost6.Checked;
                    }
                    maxShadeBiomass = Math.Max(maxShadeBiomass, maxBiomass6);


                    X0_6 = Calculations.Compute_X0(longevity6, paramD6);
                    mortR_6 = Calculations.Compute_mortR(X0_6, longevity6, paramD6);

                }

                maxShadeBiomass = Math.Max(maxShadeBiomass, landMaxBio);
                Landis.Species.Parameters parameters6 = new Landis.Species.Parameters(sppName6, longevity6, (int)maturityAge6, (byte)shadeTol6, 0, 0, 0, 0, 0, 0, PostFireRegeneration.None);
                Landis.Species.Species species6 = new Landis.Species.Species(3, parameters6);

                if (numSpecies > 5)
                {
                    speciesList.Add(species6);
                    siteMaxBio = Math.Max(siteMaxBio, maxBiomass6);
                    //Budworm
                    if(budwormHost6) hostSpeciesList.Add(species6);
                    else if (budwormCondHost6) condHostSpeciesList.Add(species6);

                }
                //Error check inputs first
                if ((rbV30.Checked)||(rbV35.Checked))
                {
                    if ((shade1 > shade2) | (shade2 > shade3) | (shade3 > shade4) | (shade4 > shade5))
                    {
                        mesg = string.Format("Minimum relative biomass threshold must increase for higher shade classes.");
                        errorCheck = false;

                        tbRelBio1.Focus();
                        tbRelBio1.SelectAll();
                    }
                }
                else if (rbAgeList.Checked)
                {
                    if ((maturityAge1 > longevity1) | (maturityAge2 > longevity2) | (maturityAge3 > longevity3))
                    {
                        mesg = string.Format("Maturity age is greater than species longevity");
                        errorCheck = false;
                    }
                }
                bool run = false;
                string var1 = "";
                int varCount = 0;
                double rangeMin = 0;
                double rangeMax = 0;
                double rangeInc = 0;
                if (cbRange.Checked && menuBatchMode.Checked)
                {
                    if (cbANPP1.Checked)
                    {
                        if (rangeANPP1)
                        {
                            var1 = "Max ANPP 1";
                            rangeMin = maxANPP1;
                        }
                        else if (rangeANPP2)
                        {
                            var1 = "Max ANPP 2";
                            rangeMin = maxANPP2;
                        }
                        else if (rangeANPP3)
                        {
                            var1 = "Max ANPP 3";
                            rangeMin = maxANPP3;
                        }
                        else if (rangeANPP4)
                        {
                            var1 = "Max ANPP 4";
                            rangeMin = maxANPP4;
                        }
                        else if (rangeANPP5)
                        {
                            var1 = "Max ANPP 5";
                            rangeMin = maxANPP5;
                        }
                        else if (rangeANPP6)
                        {
                            var1 = "Max ANPP 6";
                            rangeMin = maxANPP6;
                        }
                        varCount += 1;
                    }
                    if (cbANPP2.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeANPP1)
                            {
                                var1 = "Max ANPP 1";
                                rangeMin = maxANPP1;
                            }
                            else if (rangeANPP2)
                            {
                                var1 = "Max ANPP 2";
                                rangeMin = maxANPP2;
                            }
                            else if (rangeANPP3)
                            {
                                var1 = "Max ANPP 3";
                                rangeMin = maxANPP3;
                            }
                            else if (rangeANPP4)
                            {
                                var1 = "Max ANPP 4";
                                rangeMin = maxANPP4;
                            }
                            else if (rangeANPP5)
                            {
                                var1 = "Max ANPP 5";
                                rangeMin = maxANPP5;
                            }
                            else if (rangeANPP6)
                            {
                                var1 = "Max ANPP 6";
                                rangeMin = maxANPP6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbANPP3.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeANPP1)
                            {
                                var1 = "Max ANPP 1";
                                rangeMin = maxANPP1;
                            }
                            else if (rangeANPP2)
                            {
                                var1 = "Max ANPP 2";
                                rangeMin = maxANPP2;
                            }
                            else if (rangeANPP3)
                            {
                                var1 = "Max ANPP 3";
                                rangeMin = maxANPP3;
                            }
                            else if (rangeANPP4)
                            {
                                var1 = "Max ANPP 4";
                                rangeMin = maxANPP4;
                            }
                            else if (rangeANPP5)
                            {
                                var1 = "Max ANPP 5";
                                rangeMin = maxANPP5;
                            }
                            else if (rangeANPP6)
                            {
                                var1 = "Max ANPP 6";
                                rangeMin = maxANPP6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbANPP4.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeANPP1)
                            {
                                var1 = "Max ANPP 1";
                                rangeMin = maxANPP1;
                            }
                            else if (rangeANPP2)
                            {
                                var1 = "Max ANPP 2";
                                rangeMin = maxANPP2;
                            }
                            else if (rangeANPP3)
                            {
                                var1 = "Max ANPP 3";
                                rangeMin = maxANPP3;
                            }
                            else if (rangeANPP4)
                            {
                                var1 = "Max ANPP 4";
                                rangeMin = maxANPP4;
                            }
                            else if (rangeANPP5)
                            {
                                var1 = "Max ANPP 5";
                                rangeMin = maxANPP5;
                            }
                            else if (rangeANPP6)
                            {
                                var1 = "Max ANPP 6";
                                rangeMin = maxANPP6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbANPP5.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeANPP1)
                            {
                                var1 = "Max ANPP 1";
                                rangeMin = maxANPP1;
                            }
                            else if (rangeANPP2)
                            {
                                var1 = "Max ANPP 2";
                                rangeMin = maxANPP2;
                            }
                            else if (rangeANPP3)
                            {
                                var1 = "Max ANPP 3";
                                rangeMin = maxANPP3;
                            }
                            else if (rangeANPP4)
                            {
                                var1 = "Max ANPP 4";
                                rangeMin = maxANPP4;
                            }
                            else if (rangeANPP5)
                            {
                                var1 = "Max ANPP 5";
                                rangeMin = maxANPP5;
                            }
                            else if (rangeANPP6)
                            {
                                var1 = "Max ANPP 6";
                                rangeMin = maxANPP6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbANPP6.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeANPP1)
                            {
                                var1 = "Max ANPP 1";
                                rangeMin = maxANPP1;
                            }
                            else if (rangeANPP2)
                            {
                                var1 = "Max ANPP 2";
                                rangeMin = maxANPP2;
                            }
                            else if (rangeANPP3)
                            {
                                var1 = "Max ANPP 3";
                                rangeMin = maxANPP3;
                            }
                            else if (rangeANPP4)
                            {
                                var1 = "Max ANPP 4";
                                rangeMin = maxANPP4;
                            }
                            else if (rangeANPP5)
                            {
                                var1 = "Max ANPP 5";
                                rangeMin = maxANPP5;
                            }
                            else if (rangeANPP6)
                            {
                                var1 = "Max ANPP 6";
                                rangeMin = maxANPP6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeMM1.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeMM1)
                            {
                                var1 = "Mortality Modifier 1";
                                rangeMin = mortMod1;
                            }
                            else if (rangeMM2)
                            {
                                var1 = "Mortality Modifier 2";
                                rangeMin = mortMod2;
                            }
                            else if (rangeMM3)
                            {
                                var1 = "Mortality Modifier 3";
                                rangeMin = mortMod3;
                            }
                            else if (rangeMM4)
                            {
                                var1 = "Mortality Modifier 4";
                                rangeMin = mortMod4;
                            }
                            else if (rangeMM5)
                            {
                                var1 = "Mortality Modifier 5";
                                rangeMin = mortMod5;
                            }
                            else if (rangeMM6)
                            {
                                var1 = "Mortality Modifier 6";
                                rangeMin = mortMod6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeMM2.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeMM1)
                            {
                                var1 = "Mortality Modifier 1";
                                rangeMin = mortMod1;
                            }
                            else if (rangeMM2)
                            {
                                var1 = "Mortality Modifier 2";
                                rangeMin = mortMod2;
                            }
                            else if (rangeMM3)
                            {
                                var1 = "Mortality Modifier 3";
                                rangeMin = mortMod3;
                            }
                            else if (rangeMM4)
                            {
                                var1 = "Mortality Modifier 4";
                                rangeMin = mortMod4;
                            }
                            else if (rangeMM5)
                            {
                                var1 = "Mortality Modifier 5";
                                rangeMin = mortMod5;
                            }
                            else if (rangeMM6)
                            {
                                var1 = "Mortality Modifier 6";
                                rangeMin = mortMod6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeMM3.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeMM1)
                            {
                                var1 = "Mortality Modifier 1";
                                rangeMin = mortMod1;
                            }
                            else if (rangeMM2)
                            {
                                var1 = "Mortality Modifier 2";
                                rangeMin = mortMod2;
                            }
                            else if (rangeMM3)
                            {
                                var1 = "Mortality Modifier 3";
                                rangeMin = mortMod3;
                            }
                            else if (rangeMM4)
                            {
                                var1 = "Mortality Modifier 4";
                                rangeMin = mortMod4;
                            }
                            else if (rangeMM5)
                            {
                                var1 = "Mortality Modifier 5";
                                rangeMin = mortMod5;
                            }
                            else if (rangeMM6)
                            {
                                var1 = "Mortality Modifier 6";
                                rangeMin = mortMod6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeMM4.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeMM1)
                            {
                                var1 = "Mortality Modifier 1";
                                rangeMin = mortMod1;
                            }
                            else if (rangeMM2)
                            {
                                var1 = "Mortality Modifier 2";
                                rangeMin = mortMod2;
                            }
                            else if (rangeMM3)
                            {
                                var1 = "Mortality Modifier 3";
                                rangeMin = mortMod3;
                            }
                            else if (rangeMM4)
                            {
                                var1 = "Mortality Modifier 4";
                                rangeMin = mortMod4;
                            }
                            else if (rangeMM5)
                            {
                                var1 = "Mortality Modifier 5";
                                rangeMin = mortMod5;
                            }
                            else if (rangeMM6)
                            {
                                var1 = "Mortality Modifier 6";
                                rangeMin = mortMod6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeMM5.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeMM1)
                            {
                                var1 = "Mortality Modifier 1";
                                rangeMin = mortMod1;
                            }
                            else if (rangeMM2)
                            {
                                var1 = "Mortality Modifier 2";
                                rangeMin = mortMod2;
                            }
                            else if (rangeMM3)
                            {
                                var1 = "Mortality Modifier 3";
                                rangeMin = mortMod3;
                            }
                            else if (rangeMM4)
                            {
                                var1 = "Mortality Modifier 4";
                                rangeMin = mortMod4;
                            }
                            else if (rangeMM5)
                            {
                                var1 = "Mortality Modifier 5";
                                rangeMin = mortMod5;
                            }
                            else if (rangeMM6)
                            {
                                var1 = "Mortality Modifier 6";
                                rangeMin = mortMod6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeMM6.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeMM1)
                            {
                                var1 = "Mortality Modifier 1";
                                rangeMin = mortMod1;
                            }
                            else if (rangeMM2)
                            {
                                var1 = "Mortality Modifier 2";
                                rangeMin = mortMod2;
                            }
                            else if (rangeMM3)
                            {
                                var1 = "Mortality Modifier 3";
                                rangeMin = mortMod3;
                            }
                            else if (rangeMM4)
                            {
                                var1 = "Mortality Modifier 4";
                                rangeMin = mortMod4;
                            }
                            else if (rangeMM5)
                            {
                                var1 = "Mortality Modifier 5";
                                rangeMin = mortMod5;
                            }
                            else if (rangeMM6)
                            {
                                var1 = "Mortality Modifier 6";
                                rangeMin = mortMod6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAgeMort1.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeD1)
                            {
                                var1 = "Mort Shape 1";
                                rangeMin = paramD1;
                            }
                            else if(rangeD2)
                            {
                                var1 = "Mort Shape 2";
                                rangeMin = paramD2;
                            }
                            else if (rangeD3)
                            {
                                var1 = "Mort Shape 3";
                                rangeMin = paramD3;
                            }
                            else if (rangeD4)
                            {
                                var1 = "Mort Shape 4";
                                rangeMin = paramD4;
                            }
                            else if (rangeD5)
                            {
                                var1 = "Mort Shape 5";
                                rangeMin = paramD5;
                            }
                            else if (rangeD6)
                            {
                                var1 = "Mort Shape 6";
                                rangeMin = paramD6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAgeMort2.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeD1)
                            {
                                var1 = "Mort Shape 1";
                                rangeMin = paramD1;
                            }
                            else if (rangeD2)
                            {
                                var1 = "Mort Shape 2";
                                rangeMin = paramD2;
                            }
                            else if (rangeD3)
                            {
                                var1 = "Mort Shape 3";
                                rangeMin = paramD3;
                            }
                            else if (rangeD4)
                            {
                                var1 = "Mort Shape 4";
                                rangeMin = paramD4;
                            }
                            else if (rangeD5)
                            {
                                var1 = "Mort Shape 5";
                                rangeMin = paramD5;
                            }
                            else if (rangeD6)
                            {
                                var1 = "Mort Shape 6";
                                rangeMin = paramD6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAgeMort3.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeD1)
                            {
                                var1 = "Mort Shape 1";
                                rangeMin = paramD1;
                            }
                            else if (rangeD2)
                            {
                                var1 = "Mort Shape 2";
                                rangeMin = paramD2;
                            }
                            else if (rangeD3)
                            {
                                var1 = "Mort Shape 3";
                                rangeMin = paramD3;
                            }
                            else if (rangeD4)
                            {
                                var1 = "Mort Shape 4";
                                rangeMin = paramD4;
                            }
                            else if (rangeD5)
                            {
                                var1 = "Mort Shape 5";
                                rangeMin = paramD5;
                            }
                            else if (rangeD6)
                            {
                                var1 = "Mort Shape 6";
                                rangeMin = paramD6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAgeMort4.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeD1)
                            {
                                var1 = "Mort Shape 1";
                                rangeMin = paramD1;
                            }
                            else if (rangeD2)
                            {
                                var1 = "Mort Shape 2";
                                rangeMin = paramD2;
                            }
                            else if (rangeD3)
                            {
                                var1 = "Mort Shape 3";
                                rangeMin = paramD3;
                            }
                            else if (rangeD4)
                            {
                                var1 = "Mort Shape 4";
                                rangeMin = paramD4;
                            }
                            else if (rangeD5)
                            {
                                var1 = "Mort Shape 5";
                                rangeMin = paramD5;
                            }
                            else if (rangeD6)
                            {
                                var1 = "Mort Shape 6";
                                rangeMin = paramD6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAgeMort5.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeD1)
                            {
                                var1 = "Mort Shape 1";
                                rangeMin = paramD1;
                            }
                            else if (rangeD2)
                            {
                                var1 = "Mort Shape 2";
                                rangeMin = paramD2;
                            }
                            else if (rangeD3)
                            {
                                var1 = "Mort Shape 3";
                                rangeMin = paramD3;
                            }
                            else if (rangeD4)
                            {
                                var1 = "Mort Shape 4";
                                rangeMin = paramD4;
                            }
                            else if (rangeD5)
                            {
                                var1 = "Mort Shape 5";
                                rangeMin = paramD5;
                            }
                            else if (rangeD6)
                            {
                                var1 = "Mort Shape 6";
                                rangeMin = paramD6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAgeMort6.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeD1)
                            {
                                var1 = "Mort Shape 1";
                                rangeMin = paramD1;
                            }
                            else if (rangeD2)
                            {
                                var1 = "Mort Shape 2";
                                rangeMin = paramD2;
                            }
                            else if (rangeD3)
                            {
                                var1 = "Mort Shape 3";
                                rangeMin = paramD3;
                            }
                            else if (rangeD4)
                            {
                                var1 = "Mort Shape 4";
                                rangeMin = paramD4;
                            }
                            else if (rangeD5)
                            {
                                var1 = "Mort Shape 5";
                                rangeMin = paramD5;
                            }
                            else if (rangeD6)
                            {
                                var1 = "Mort Shape 6";
                                rangeMin = paramD6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAP1.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeAP1)
                            {
                                var1 = "ANPP Power 1";
                                rangeMin = anppPower1;
                            }
                            else if(rangeAP2)
                            {
                                var1 = "ANPP Power 2";
                                rangeMin = anppPower2;
                            }
                            else if (rangeAP3)
                            {
                                var1 = "ANPP Power 3";
                                rangeMin = anppPower3;
                            }
                            else if (rangeAP4)
                            {
                                var1 = "ANPP Power 4";
                                rangeMin = anppPower4;
                            }
                            else if (rangeAP5)
                            {
                                var1 = "ANPP Power 5";
                                rangeMin = anppPower5;
                            }
                            else if (rangeAP6)
                            {
                                var1 = "ANPP Power 6";
                                rangeMin = anppPower6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAP2.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeAP1)
                            {
                                var1 = "ANPP Power 1";
                                rangeMin = anppPower1;
                            }
                            else if (rangeAP2)
                            {
                                var1 = "ANPP Power 2";
                                rangeMin = anppPower2;
                            }
                            else if (rangeAP3)
                            {
                                var1 = "ANPP Power 3";
                                rangeMin = anppPower3;
                            }
                            else if (rangeAP4)
                            {
                                var1 = "ANPP Power 4";
                                rangeMin = anppPower4;
                            }
                            else if (rangeAP5)
                            {
                                var1 = "ANPP Power 5";
                                rangeMin = anppPower5;
                            }
                            else if (rangeAP6)
                            {
                                var1 = "ANPP Power 6";
                                rangeMin = anppPower6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAP3.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeAP1)
                            {
                                var1 = "ANPP Power 1";
                                rangeMin = anppPower1;
                            }
                            else if (rangeAP2)
                            {
                                var1 = "ANPP Power 2";
                                rangeMin = anppPower2;
                            }
                            else if (rangeAP3)
                            {
                                var1 = "ANPP Power 3";
                                rangeMin = anppPower3;
                            }
                            else if (rangeAP4)
                            {
                                var1 = "ANPP Power 4";
                                rangeMin = anppPower4;
                            }
                            else if (rangeAP5)
                            {
                                var1 = "ANPP Power 5";
                                rangeMin = anppPower5;
                            }
                            else if (rangeAP6)
                            {
                                var1 = "ANPP Power 6";
                                rangeMin = anppPower6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAP4.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeAP1)
                            {
                                var1 = "ANPP Power 1";
                                rangeMin = anppPower1;
                            }
                            else if (rangeAP2)
                            {
                                var1 = "ANPP Power 2";
                                rangeMin = anppPower2;
                            }
                            else if (rangeAP3)
                            {
                                var1 = "ANPP Power 3";
                                rangeMin = anppPower3;
                            }
                            else if (rangeAP4)
                            {
                                var1 = "ANPP Power 4";
                                rangeMin = anppPower4;
                            }
                            else if (rangeAP5)
                            {
                                var1 = "ANPP Power 5";
                                rangeMin = anppPower5;
                            }
                            else if (rangeAP6)
                            {
                                var1 = "ANPP Power 6";
                                rangeMin = anppPower6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAP5.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeAP1)
                            {
                                var1 = "ANPP Power 1";
                                rangeMin = anppPower1;
                            }
                            else if (rangeAP2)
                            {
                                var1 = "ANPP Power 2";
                                rangeMin = anppPower2;
                            }
                            else if (rangeAP3)
                            {
                                var1 = "ANPP Power 3";
                                rangeMin = anppPower3;
                            }
                            else if (rangeAP4)
                            {
                                var1 = "ANPP Power 4";
                                rangeMin = anppPower4;
                            }
                            else if (rangeAP5)
                            {
                                var1 = "ANPP Power 5";
                                rangeMin = anppPower5;
                            }
                            else if (rangeAP6)
                            {
                                var1 = "ANPP Power 6";
                                rangeMin = anppPower6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (cbRangeAP6.Checked)
                    {
                        if (var1 == "")
                        {
                            if (rangeAP1)
                            {
                                var1 = "ANPP Power 1";
                                rangeMin = anppPower1;
                            }
                            else if (rangeAP2)
                            {
                                var1 = "ANPP Power 2";
                                rangeMin = anppPower2;
                            }
                            else if (rangeAP3)
                            {
                                var1 = "ANPP Power 3";
                                rangeMin = anppPower3;
                            }
                            else if (rangeAP4)
                            {
                                var1 = "ANPP Power 4";
                                rangeMin = anppPower4;
                            }
                            else if (rangeAP5)
                            {
                                var1 = "ANPP Power 5";
                                rangeMin = anppPower5;
                            }
                            else if (rangeAP6)
                            {
                                var1 = "ANPP Power 6";
                                rangeMin = anppPower6;
                            }
                            varCount += 1;
                        }
                        else
                        {
                            varCount += 1;
                        }
                    }
                    if (varCount > 1)
                    {
                        run = false;
                        mesg = string.Format("More than 1 range variable selected");
                        errorCheck = false;
                    }
                    else if (varCount == 0)
                    {
                        run = false;
                        mesg = string.Format("No range variable selected");
                        errorCheck = false;
                    }

                    else
                    {
                        Form2 inputForm = new Form2();
                        inputForm.labelParam.Text = var1;
                        inputForm.tbMin.Text = rangeMin.ToString();
                        inputForm.tbMax.Text = rangeMin.ToString();
                        inputForm.ShowDialog();
                        run = inputForm.checkBox1.Checked;
                        rangeMin = double.Parse(inputForm.tbMin.Text);
                        rangeMax = double.Parse(inputForm.tbMax.Text);
                        rangeInc = double.Parse(inputForm.tbIncrement.Text);

                    }
                }
                else
                {
                    run = true;
                    rangeMin = 0;
                    rangeMax = 0;
                    rangeInc = 1;
                }


                if (!errorCheck)
                {
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                else
                {
                    if (run)
                    {
                        int rangeSum = 0;
                        for (double rangeC = rangeMin; rangeC <= rangeMax; rangeC += rangeInc)
                        {
                            rangeSum++;
                        }
                        rangeCount = 0;
                        for (double range = rangeMin; range <= rangeMax; range += rangeInc)
                        {
                            if (cbRange.Checked && menuBatchMode.Checked)
                            {
                                if (var1 == "ANPP Peak 1")
                                    anppPeak1 = range;
                                else if (var1 == "ANPP Peak 2")
                                    anppPeak2 = range;
                                else if (var1 == "ANPP Peak 3")
                                    anppPeak3 = range;
                                else if (var1 == "ANPP Peak 4")
                                    anppPeak4 = range;
                                else if (var1 == "ANPP Peak 5")
                                    anppPeak5 = range;
                                else if (var1 == "ANPP Peak 6")
                                    anppPeak6 = range;
                                else if (var1 == "Max ANPP 1")
                                    maxANPP1 = (int) range;
                                else if (var1 == "Max ANPP 2")
                                    maxANPP2 = (int)range;
                                else if (var1 == "Max ANPP 3")
                                    maxANPP3 = (int)range;
                                else if (var1 == "Max ANPP 4")
                                    maxANPP4 = (int)range;
                                else if (var1 == "Max ANPP 5")
                                    maxANPP5 = (int)range;
                                else if (var1 == "Max ANPP 6")
                                    maxANPP6 = (int)range;
                                else if (var1 == "Landscape Max ANPP 1")
                                {
                                    maxANPP1 = (int)range;
                                    shadeANPP1 = (int)range;
                                }
                                else if (var1 == "Landscape Max ANPP 2")
                                {
                                    maxANPP2 = (int)range;
                                    shadeANPP2 = (int)range;
                                }
                                else if (var1 == "Landscape Max ANPP 3")
                                {
                                    maxANPP3 = (int)range;
                                    shadeANPP3 = (int)range;
                                }
                                else if (var1 == "Landscape Max ANPP 4")
                                {
                                    maxANPP4 = (int)range;
                                    shadeANPP4 = (int)range;
                                }
                                else if (var1 == "Landscape Max ANPP 5")
                                {
                                    maxANPP5 = (int)range;
                                    shadeANPP5 = (int)range;
                                }
                                else if (var1 == "Landscape Max ANPP 6")
                                {
                                    maxANPP6 = (int)range;
                                    shadeANPP6 = (int)range;
                                }
                                else if (var1 == "Mortality Modifier 1")
                                {
                                    mortMod1 = range;
                                }
                                else if (var1 == "Mortality Modifier 2")
                                {
                                    mortMod2 = range;
                                }
                                else if (var1 == "Mortality Modifier 3")
                                {
                                    mortMod3 = range;
                                }
                                else if (var1 == "Mortality Modifier 4")
                                {
                                    mortMod4 = range;
                                }
                                else if (var1 == "Mortality Modifier 5")
                                {
                                    mortMod5 = range;
                                }
                                else if (var1 == "Mortality Modifier 6")
                                {
                                    mortMod6 = range;
                                }
                                else if (var1 == "Thinning Coefficient 1")
                                    thinCoeff1 = range;
                                else if (var1 == "Thinning Coefficient 2")
                                    thinCoeff2 = range;
                                else if (var1 == "Thinning Coefficient 3")
                                    thinCoeff3 = range;
                                else if (var1 == "Thinning Coefficient 4")
                                    thinCoeff4 = range;
                                else if (var1 == "Thinning Coefficient 5")
                                    thinCoeff5 = range;
                                else if (var1 == "Thinning Coefficient 6")
                                    thinCoeff6 = range;
                                else if (var1 == "Mort Shape 1")
                                {
                                    paramD1 = range;
                                    X0_1 = Calculations.Compute_X0(longevity1, paramD1);
                                    mortR_1 = Calculations.Compute_mortR(X0_1, longevity1, paramD1);
                                }
                                else if (var1 == "Mort Shape 2")
                                {
                                    paramD2 = range;
                                    X0_2 = Calculations.Compute_X0(longevity2, paramD2);
                                    mortR_2 = Calculations.Compute_mortR(X0_2, longevity2, paramD2);
                                }
                                else if (var1 == "Mort Shape 3")
                                {
                                    paramD3 = range;
                                    X0_3 = Calculations.Compute_X0(longevity3, paramD3);
                                    mortR_3 = Calculations.Compute_mortR(X0_3, longevity3, paramD3);
                                }
                                else if (var1 == "Mort Shape 4")
                                {
                                    paramD4 = range;
                                    X0_4 = Calculations.Compute_X0(longevity4, paramD4);
                                    mortR_4 = Calculations.Compute_mortR(X0_4, longevity4, paramD4);
                                }
                                else if (var1 == "Mort Shape 5")
                                {
                                    paramD5 = range;
                                    X0_5 = Calculations.Compute_X0(longevity5, paramD5);
                                    mortR_5 = Calculations.Compute_mortR(X0_5, longevity5, paramD5);
                                }
                                else if (var1 == "Mort Shape 6")
                                {
                                    paramD6 = range;
                                    X0_6 = Calculations.Compute_X0(longevity6, paramD6);
                                    mortR_6 = Calculations.Compute_mortR(X0_6, longevity6, paramD6);
                                }
                                else if (var1 == "ANPP Power 1")
                                    anppPower1 = range;
                                else if (var1 == "ANPP Power 2")
                                    anppPower2 = range;
                                else if (var1 == "ANPP Power 3")
                                    anppPower3 = range;
                                else if (var1 == "ANPP Power 4")
                                    anppPower4 = range;
                                else if (var1 == "ANPP Power 5")
                                    anppPower5 = range;
                                else if (var1 == "ANPP Power 6")
                                    anppPower6 = range;

                            }
                            double scale1 = 1;
                            double scale2 = 1;
                            double scale3 = 1;
                            double scale4 = 1;
                            double scale5 = 1;
                            double scale6 = 1;

                            Random random = new Random(randSeed);
                            double rangeProp = (double)rangeCount / (double)rangeSum;
                            for (int r = 1; r <= batchNum; r++)
                            {
                                Color c = Color.FromArgb(
                                    127,
                                     (byte)random.Next(0, 256),
                                     (byte)random.Next(0, 256),
                                     (byte)random.Next(0, 256));
                                if (menuBatchMode.Checked)
                                    if (cbRange.Checked)
                                    {
                                        int redInt = (int) (rangeProp * 255);
                                        int greenInt = (int) ((1 - rangeProp) * 255);

                                        c = Color.FromArgb(20, redInt, greenInt, 0);
                                    }
                                    else
                                    {
                                        c = Color.FromArgb(2, c.R, c.G, c.B);
                                    }
                                string filename = baseFileName+"_Out" + repCount.ToString() + ".csv";
                                string paramFileName = baseFileName+ "_Param" + repCount.ToString() + ".csv";
                                string outTextFile = Path.Combine(dirName, filename);
                                string paramTextFile = Path.Combine(dirName, paramFileName);

                                repCount += 1;



                                double[] bioSumArray = new double[simYears + 1];
                                double[] bioSumArray1 = new double[simYears + 1];
                                double[] pctShadeArray = new double[simYears + 1];
                                double[] shadeClassArray = new double[simYears + 1];
                                double[] numCohortsArray = new double[simYears + 1];
                                double[] bioSumArray2 = new double[simYears + 1];
                                double[] pctShadeArray2 = new double[simYears + 1];
                                double[] numCohortsArray2 = new double[simYears + 1];
                                double[] bioSumArray3 = new double[simYears + 1];
                                double[] pctShadeArray3 = new double[simYears + 1];
                                double[] numCohortsArray3 = new double[simYears + 1];
                                double[] bioSumArray4 = new double[simYears + 1];
                                double[] pctShadeArray4 = new double[simYears + 1];
                                double[] numCohortsArray4 = new double[simYears + 1];
                                double[] bioSumArray5 = new double[simYears + 1];
                                double[] pctShadeArray5 = new double[simYears + 1];
                                double[] numCohortsArray5 = new double[simYears + 1];
                                double[] bioSumArray6 = new double[simYears + 1];
                                double[] pctShadeArray6 = new double[simYears + 1];
                                double[] numCohortsArray6 = new double[simYears + 1];
                                double[] mortSumArray = new double[simYears + 11];
                                double[] cohortArray1 = new double[simYears + 11];
                                double[] cohortArray2 = new double[simYears + 11];
                                double[] LAIArray1 = new double[simYears + 11];
                                double[] LAIArray2 = new double[simYears + 11];
                                double[] LAIArray3 = new double[simYears + 11];
                                double[] LAIArray4 = new double[simYears + 11];
                                double[] LAIArray5 = new double[simYears + 11];
                                double[] LAIArray6 = new double[simYears + 11];
                                double[] LAITotalArray = new double[simYears + 11];
                                double[] ANPPArray1 = new double[simYears + 11];
                                double[] ANPPArray2 = new double[simYears + 11];
                                double[] ANPPArray3 = new double[simYears + 11];
                                double[] ANPPArray4 = new double[simYears + 11];
                                double[] ANPPArray5 = new double[simYears + 11];
                                double[] ANPPArray6 = new double[simYears + 11];
                                double[] MortAgeArray1 = new double[simYears + 11];
                                double[] MortAgeArray2 = new double[simYears + 11];
                                double[] MortAgeArray3 = new double[simYears + 11];
                                double[] MortAgeArray4 = new double[simYears + 11];
                                double[] MortAgeArray5 = new double[simYears + 11];
                                double[] MortAgeArray6 = new double[simYears + 11];
                                double[] MortCompArray1 = new double[simYears + 11];
                                double[] MortCompArray2 = new double[simYears + 11];
                                double[] MortCompArray3 = new double[simYears + 11];
                                double[] MortCompArray4 = new double[simYears + 11];
                                double[] MortCompArray5 = new double[simYears + 11];
                                double[] MortCompArray6 = new double[simYears + 11];
                                double[] CoBioArray1 = new double[simYears + 11];
                                double[] CoBioArray2 = new double[simYears + 11];
                                double[] CoBioArray3 = new double[simYears + 11];
                                double[] CoBioArray4 = new double[simYears + 11];
                                double[] CoBioArray5 = new double[simYears + 11];
                                double[] CoBioArray6 = new double[simYears + 11];
                                double[] deadWoodArray = new double[simYears + 11];
                                double[] layersArray = new double[simYears + 11];
                                double[] paraArray = new double[simYears + 11];
                                //Budworm - set up arrays to record values
                                double[] budwormDensArray = new double[simYears + 11];
                                double[] budwormCountArray = new double[simYears + 11];
                                double[] budEnemyDensArray = new double[simYears + 11];
                                double[] budEnemyCountArray = new double[simYears + 11];
                                double[] pctDefoliationArray = new double[simYears + 11];
                                double[] siteDefoliationArray = new double[simYears + 11];
                                double[] hostFoliageArray = new double[simYears + 11];
                                double[] currentHostFoliageArray = new double[simYears + 11];
                                double[] hostFoliageFallArray = new double[simYears + 11];
                                double[] currentHostFoliageFallArray = new double[simYears + 11];
                                double[] budwormMortalityArray = new double[simYears + 11];
                                double[] emigrationArray = new double[simYears + 11];
                                //Budworm - END - set up arrays to record values

                                //Create cohort list
                                List<Cohort> cohortList = new List<Cohort>();

                                //Budworm
                                //Create defol history list to coincide with cohort list
                                List<double[]> defolHistList = new List<double[]>();
                                //Create foliage list to coincide with cohort list
                                List<int> totalFoliageList = new List<int>();
                                //Create current foliage list to coincide with cohort list
                                List<int> currentFoliageList = new List<int>();
                                //Create wood biomass list to coincide with cohort list
                                List<double> woodList = new List<double>();

                                

                                //Cohort Biomass Array
                                List<double[]> coBioList1 = new List<double[]>();
                                coBioList1.Add(CoBioArray1);
                                List<double[]> coBioList2 = new List<double[]>();
                                coBioList2.Add(CoBioArray2);
                                List<double[]> coBioList3 = new List<double[]>();
                                coBioList3.Add(CoBioArray3);
                                List<double[]> coBioList4 = new List<double[]>();
                                coBioList4.Add(CoBioArray4);
                                List<double[]> coBioList5 = new List<double[]>();
                                coBioList5.Add(CoBioArray5);
                                List<double[]> coBioList6 = new List<double[]>();
                                coBioList6.Add(CoBioArray6);

                                List<List<double[]>> coBioList = new List<List<double[]>>();
                                coBioList.Add(coBioList1);
                                if (numSpecies > 1)
                                    coBioList.Add(coBioList2);
                                if (numSpecies > 2)
                                    coBioList.Add(coBioList3);
                                if (numSpecies > 3)
                                    coBioList.Add(coBioList4);
                                if (numSpecies > 4)
                                    coBioList.Add(coBioList5);
                                if (numSpecies > 5)
                                    coBioList.Add(coBioList6);

                                //LAI Array
                                List<double[]> coList1 = new List<double[]>();
                                coList1.Add(LAIArray1);
                                List<double[]> coList2 = new List<double[]>();
                                coList2.Add(LAIArray2);
                                List<double[]> coList3 = new List<double[]>();
                                coList3.Add(LAIArray3);
                                List<double[]> coList4 = new List<double[]>();
                                coList4.Add(LAIArray4);
                                List<double[]> coList5 = new List<double[]>();
                                coList5.Add(LAIArray5);
                                List<double[]> coList6 = new List<double[]>();
                                coList6.Add(LAIArray6);

                                List<List<double[]>> coList = new List<List<double[]>>();
                                coList.Add(coList1);
                                if (numSpecies > 1)
                                    coList.Add(coList2);
                                if (numSpecies > 2)
                                    coList.Add(coList3);
                                if (numSpecies > 3)
                                    coList.Add(coList4);
                                if (numSpecies > 4)
                                    coList.Add(coList5); 
                                if (numSpecies > 5)
                                    coList.Add(coList6);

                                //ANPP Array
                                List<double[]> ANPPCoList1 = new List<double[]>();
                                ANPPCoList1.Add(ANPPArray1);
                                List<double[]> ANPPCoList2 = new List<double[]>();
                                ANPPCoList2.Add(ANPPArray2);
                                List<double[]> ANPPCoList3 = new List<double[]>();
                                ANPPCoList3.Add(ANPPArray3);
                                List<double[]> ANPPCoList4 = new List<double[]>();
                                ANPPCoList4.Add(ANPPArray4);
                                List<double[]> ANPPCoList5 = new List<double[]>();
                                ANPPCoList5.Add(ANPPArray5);
                                List<double[]> ANPPCoList6 = new List<double[]>();
                                ANPPCoList6.Add(ANPPArray6);

                                List<List<double[]>> ANPPCoList = new List<List<double[]>>();
                                ANPPCoList.Add(ANPPCoList1);
                                if (numSpecies > 1)
                                    ANPPCoList.Add(ANPPCoList2);
                                if (numSpecies > 2)
                                    ANPPCoList.Add(ANPPCoList3);
                                if (numSpecies > 3)
                                    ANPPCoList.Add(ANPPCoList4);
                                if (numSpecies > 4)
                                    ANPPCoList.Add(ANPPCoList5);
                                if (numSpecies > 5)
                                    ANPPCoList.Add(ANPPCoList6);

                                //Age Mort Array
                                List<double[]> MortAgeCoList1 = new List<double[]>();
                                MortAgeCoList1.Add(MortAgeArray1);
                                List<double[]> MortAgeCoList2 = new List<double[]>();
                                MortAgeCoList2.Add(MortAgeArray2);
                                List<double[]> MortAgeCoList3 = new List<double[]>();
                                MortAgeCoList3.Add(MortAgeArray3);
                                List<double[]> MortAgeCoList4 = new List<double[]>();
                                MortAgeCoList4.Add(MortAgeArray4);
                                List<double[]> MortAgeCoList5 = new List<double[]>();
                                MortAgeCoList5.Add(MortAgeArray5);
                                List<double[]> MortAgeCoList6 = new List<double[]>();
                                MortAgeCoList6.Add(MortAgeArray6);

                                List<List<double[]>> MortAgeCoList = new List<List<double[]>>();
                                MortAgeCoList.Add(MortAgeCoList1);
                                if (numSpecies > 1)
                                    MortAgeCoList.Add(MortAgeCoList2);
                                if (numSpecies > 2)
                                    MortAgeCoList.Add(MortAgeCoList3);
                                if (numSpecies > 3)
                                    MortAgeCoList.Add(MortAgeCoList4);
                                if (numSpecies > 4)
                                    MortAgeCoList.Add(MortAgeCoList5);
                                if (numSpecies > 5)
                                    MortAgeCoList.Add(MortAgeCoList6);

                                //Comp Mort Array
                                List<double[]> MortCompCoList1 = new List<double[]>();
                                MortCompCoList1.Add(MortCompArray1);
                                List<double[]> MortCompCoList2 = new List<double[]>();
                                MortCompCoList2.Add(MortCompArray2);
                                List<double[]> MortCompCoList3 = new List<double[]>();
                                MortCompCoList3.Add(MortCompArray3);
                                List<double[]> MortCompCoList4 = new List<double[]>();
                                MortCompCoList4.Add(MortCompArray4);
                                List<double[]> MortCompCoList5 = new List<double[]>();
                                MortCompCoList5.Add(MortCompArray5);
                                List<double[]> MortCompCoList6 = new List<double[]>();
                                MortCompCoList6.Add(MortCompArray6);

                                List<List<double[]>> MortCompCoList = new List<List<double[]>>();
                                MortCompCoList.Add(MortCompCoList1);
                                if (numSpecies > 1)
                                    MortCompCoList.Add(MortCompCoList2);
                                if (numSpecies > 2)
                                    MortCompCoList.Add(MortCompCoList3);
                                if (numSpecies > 3)
                                    MortCompCoList.Add(MortCompCoList4);
                                if (numSpecies > 4)
                                    MortCompCoList.Add(MortCompCoList5);
                                if (numSpecies > 5)
                                    MortCompCoList.Add(MortCompCoList6);

                                double bioSum1 = 0;
                                double bioSum2 = 0;
                                double bioSum3 = 0;
                                double bioSum4 = 0;
                                double bioSum5 = 0;
                                double bioSum6 = 0;
                                double deadWoodyBio = 0;
                                double totalDecayRate = 0;
                                //Year 0 biomass initialized
                                foreach (ISpecies spp in speciesList)
                                {
                                    int maxANPP = 0;
                                    if (spp == species1)
                                    {
                                        maxANPP = maxANPP1;
                                    }
                                    else if (spp == species2)
                                    {
                                        maxANPP = maxANPP2;
                                    }
                                    else if (spp == species3)
                                    {
                                        maxANPP = maxANPP3;
                                    }
                                    else if (spp == species4)
                                    {
                                        maxANPP = maxANPP4;
                                    }
                                    else if (spp == species5)
                                    {
                                        maxANPP = maxANPP5;
                                    }
                                    else if (spp == species6)
                                    {
                                        maxANPP = maxANPP6;
                                    }
                                }

                                double prevMort = 0;
                                double bioSumTotal = 0;
                                //Budworm
                                // set densities to input year 0 values
                                // declare Count parameters
                                // reset cumulative defoliation to 0;
                                double budEnemyDensity = budEnemyDensity0;
                                double budwormDensity = budwormDensity0;
                                double budwormCount = 0;
                                double enemyCount = 0;
                                //double sumDefoliation = 0;
                                double currentHostFoliage = 0;  // Current host foliage by site
                                double hostFoliage = 0;   // Host foliage by site
                                double totalFoliage = 0;  //Total foliage by cohort
                                double currentFoliage = 0;  //Current foliage by cohort
                                double totalFoliageAfterDefol = 0; //Total foliage by cohort after defoliation
                                double totalFoliageAfterMort = 0; //Total foliage by cohort after mortality
                                int maxHostAge = 0;
                                //Budworm - END - set densities to input year 0 values & declare Count parameters

                                for (int year = 0; year <= simYears + timestep; year++)
                                {
                                    string msg = "";
                                    if ((year - timestep) == 77)
                                        msg = "year 77)";
                                    byte shadeClass = 0;
                                    double pctShade = 0;
                                    double newBioSum1 = bioSum1;
                                    double newBioSum2 = bioSum2;
                                    double newBioSum3 = bioSum3;
                                    double newBioSum4 = bioSum4;
                                    double newBioSum5 = bioSum5;
                                    double newBioSum6 = bioSum6;
                                    double bioSum = 0;
                                    int removeSum1 = 0;
                                    int removeSum2 = 0;
                                    int removeSum3 = 0;
                                    int removeSum4 = 0;
                                    int removeSum5 = 0;
                                    int removeSum6 = 0;
                                    int removeSum = 0;
                                    double initBiomass = 0;
                                    double initBiomass2 = 0;
                                    double initBiomass3 = 0;
                                    double initBiomass4 = 0;
                                    double initBiomass5 = 0;
                                    double initBiomass6 = 0;
                                    double currentMort = 0;
                                    int cohortCount1 = 0;
                                    int cohortCount2 = 0;
                                    int cohortCount3 = 0;
                                    int cohortCount4 = 0;
                                    int cohortCount5 = 0;
                                    int cohortCount6 = 0;
                                    double cohort1Bio = 0;
                                    double cohort2Bio = 0;
                                    double maxCohortBio = 0;
                                    double secondCohortBio = 0;
                                    List<Cohort> newCohortList = new List<Cohort>();
                                    List<Cohort> renewCohortList = new List<Cohort>();
                                    //Budworm - create cohort attributes
                                    //Create defol history list to coincide with cohort list
                                    List<double[]> newDefolHistList = new List<double[]>();
                                    //Create foliage list to coincide with cohort list
                                    List<int> newTotalFoliageList = new List<int>();
                                    //Create current foliage list to coincide with cohort list
                                    List<int> newCurrentFoliageList = new List<int>();
                                    //Create wood biomass list to coincide with cohort list
                                    List<double> newWoodList = new List<double>();
                                    //maxHostAge = 0;
                                    int totalSBWMortality = 0;
                                    //Budworm - END - create cohort attributes

                                    
                                    //Budworm - calculate deciduous biomass
                                    double decidBiomass = 0;
                                    if (budwormDecid1)
                                        decidBiomass += bioSum1;
                                    if (budwormDecid2)
                                        decidBiomass += bioSum2;
                                    if (budwormDecid3)
                                        decidBiomass += bioSum3;
                                    if (budwormDecid4)
                                        decidBiomass += bioSum4;
                                    if (budwormDecid5)
                                        decidBiomass += bioSum5;
                                    if (budwormDecid6)
                                        decidBiomass += bioSum6;


                                    //Budworm - END - calculate deciduous biomass
                                    double siteBiomass = bioSum1 + bioSum2 + bioSum3 + bioSum4 + bioSum5 + bioSum6;
                                    bool spp1Renew = false;
                                    bool spp2Renew = false;
                                    bool spp3Renew = false;
                                    bool spp4Renew = false;
                                    bool spp5Renew = false;
                                    bool spp6Renew = false;
                                    bool removeBool1 = false;
                                    bool removeBool2 = false;
                                    bool removeBool3 = false;
                                    bool removeBool4 = false;
                                    bool removeBool5 = false;
                                    bool removeBool6 = false;
                                    bool resproutBool1 = false;
                                    bool resproutBool2 = false;
                                    bool resproutBool3 = false;
                                    bool resproutBool4 = false;
                                    bool resproutBool5 = false;
                                    bool resproutBool6 = false;
                                    int spp1Count = 0;
                                    int spp2Count = 0;
                                    int spp3Count = 0;
                                    int spp4Count = 0;
                                    int spp5Count = 0;
                                    int spp6Count = 0;
                                    int groupIndex = 0;
                                    int lastGroupIndex = 0;
                                    int lastGroupShadeTol = 0;
                                    

                                    //Budworm - Start annual calculations
                                    // draw annual stochastic variables from normal distributions
                                    // override to equal mean if stdev was 0
                                    // these will be spatially auto-correlated functions in the real LANDIS-II extension
                                    double winterSurvival = normDistWinter.NextDouble();
                                    if (budwormWinterConstant)
                                        winterSurvival = budwormWinterMean;
                                    double randPredation = normDistPredation.NextDouble();
                                    if (budwormPredationConstant)
                                        randPredation = budwormPredationMean;
                                    double phenolLimit = normDistPhenolLimit.NextDouble();
                                    if (phenolLimitConstant)
                                        phenolLimit = phenolLimitMean;

                                    if ((year - timestep) == budwormStartYear) // at simulation year 0 use density values from the parameter inputs
                                    {
                                        budwormCount = budwormDensity0 * currentHostFoliage;
                                        enemyCount = budEnemyDensity0 * budwormCount;
                                    }
                                    else if ((year - timestep) < budwormStartYear)
                                    {
                                        budwormCount = 0;
                                        enemyCount = 0;
                                    }
                                   

                                    // apply stochastic winter survival (3)
                                    double budwormCountSpring = budwormCount * winterSurvival;

                                // spatial average filter budworm counts to simulate L2 dispersal (4) - not applied in L2-Site
                                double filteredBudwormSpring = budwormCountSpring;

                                // calculate budworm density (5)
                                double filteredDensitySpring = 0;
                                if (currentHostFoliage > 0)
                                    filteredDensitySpring = filteredBudwormSpring / currentHostFoliage;
                                else
                                {
                                    if (filteredBudwormSpring > 0)
                                        filteredDensitySpring = budwormMaxDensity;
                                }
                                double budwormDensityL2_yt = filteredDensitySpring;

                                // spatial filter-average to simulate natural enemy dispersal (6)
                                double filteredEnemyCount = enemyCount;

                                 // Rescale budworm density (7)
                                 double budwormDensityL2Scaled = Math.Pow((budwormDensityL2_yt / preyM), (1.0 / preyN));

                                 // Convert L2 budworm density to counts (8)
                                 double budwormCountL2 = budwormDensityL2Scaled * currentHostFoliage;

                                if ((year - timestep) == budwormStartYear) // at simulation year 0 use density values from the parameter inputs
                                {
                                    enemyCount = budEnemyDensity0 * budwormCountSpring;
                                }
                                else if ((year - timestep) < budwormStartYear)
                                {
                                    enemyCount = 0;
                                }

                                    // Calculate spring enemy density (9)
                                    double enemyDensitySpring_xt = 0;
                                    if (budwormCountSpring > 0)
                                    enemyDensitySpring_xt = enemyCount / budwormCountSpring;
                                    // Rescale enemy density (10)
                                    double enemyDensitySpringScaled = Math.Pow((enemyDensitySpring_xt / predM), (1.0 / predN));
     
                                    // calculate mating effect (11a)
                                    //double alleeEffect = Calculations.HillFunction(alleeShape, alleeHalfSat, budwormDensityL2);
                                    double matingEffect = Calculations.ProportionMatedFunction(matingA, matingB, matingC, budwormDensityL2_yt);

                                    // calculate deciduous protection effect (11b)
                                    double decidProtect1 = 0;
                                    double decidProtect2 = 0;
                                    if (siteBiomass > 0)
                                    {
                                        decidProtect1 = decidBiomass / siteBiomass * decidProtectDmax1;  // dispersal loss effect
                                        decidProtect2 = decidBiomass / siteBiomass * decidProtectDmax2;  // parasite community composition effect
                                    }

                                    // recruitment functions (12)
                                    double ryx = 1 - Math.Exp((-1 * budwormb * budwormDensityL2_yt) - decidProtect2); // confirmed with spreadsheet
                                    double rxy = Math.Exp(-1 * budwormc * enemyDensitySpring_xt); // confirmed with spreadsheet
                                    double rt = budwormrm * ryx * rxy;  //confirmed with spreadsheet
                                    double fecundity = 216.8;

                                    // calculate r't (rprimet) without foliage dependence
                                    double rprimeyx = Math.Exp((-1 * (budwormbprime + decidProtect1) * Math.Pow(budwormDensityL2_yt, budwormaprime)));  //confirmed with spreadsheet
                                    double rprimexy = Math.Exp(-1 * (budwormcprime * enemyDensitySpring_xt));  //confirmed with spreadsheet
                                    double rprimet = fecundity * budwormrprimem * rprimeyx * rprimexy * matingEffect;  //confirmed with spreadsheet

                                    // Host Tree Damage
                                    // Calculate defoliation (15)
                                    // Use Regniere and You 1991; Eq. 13
                                    //double defolPopulationComp = 0.385; // Product of indivudual instar [lambda + (1-lambda)*S]
                                    double allLarvalSurvival = rprimeyx * rprimexy;
                                    //double defolPopulationComp = (defolLambda + (1.0-defolLambda)* allLarvalSurvival); // Product of indivudual instar [lambda + (1-lambda)*S]
                                    double defolPopulationComp = (defolLambda + (1.0 - defolLambda) * 1);  //confirmed with spreadsheet if defolLambda = 0.255
                                    double etaDefol = 870; // mg foliage removed per budworm
                                    double pctDefol = 100 * etaDefol * 0.001 * budwormDensityL2_yt * defolPopulationComp; //convert mg to g (0.001)
                                    // cap defoliation at 100%
                                    pctDefol = Math.Min(pctDefol, 100);
                                    // do not allow damage if no budworm
                                    if (!(budwormCountL2 > 0))
                                        pctDefol = 0;

                                    // calculate r''t (rprime2t) with defoliation effect on fecundity
                                    // calculate defoliation effect on fecundity
                                    double rprimeZ = Calculations.CalculateRprimeZ(pctDefol); //Nealis & Regniere 2004, Fig 2

                                    double rprime2t = fecundity * budwormrprimem * rprimeZ * rprimeyx * rprimexy * matingEffect;  //confirmed with spreadsheet

                                    // calculate enemy density following recruitment (12)
                                    double enemyDensitySummer = enemyDensitySpringScaled * rt;

                                    // calculate budworm density following recruitment 12)
                                    double budwormDensitySummer = 0;
                                    if (cbDefolFecund.Checked)  // Defoliation-adjusted fecundity
                                    {
                                        if (year < timestep)
                                            budwormDensitySummer = Math.Max(budwormDensityL2_yt * rprime2t, budwormDensity0); // Don't allow to go to 0 during spin-up
                                        else
                                            budwormDensitySummer = Math.Max(budwormDensityL2_yt * rprime2t, 0);
                                    }
                                    else  // No Defoliation-adjusted fecundity
                                    {
                                        if (year < timestep)
                                            budwormDensitySummer = Math.Max(budwormDensityL2_yt * rprimet, budwormDensity0); // Don't allow to go to 0 during spin-up
                                        else
                                            budwormDensitySummer = Math.Max(budwormDensityL2_yt * rprimet, 0);
                                    }

                                   
                                    // calculate number of budworm and enemies (13)
                                    double budwormCountSummer = budwormDensitySummer * currentHostFoliage;
                                    double enemyCountWinter = enemyDensitySummer * budwormCountSummer;
                                    enemyCount = enemyCountWinter;

                                    // apply stochastic predation (14a)
                                    double budwormCountFall = budwormCountSummer * randPredation;

                                    // apply stochastic phenological limitation (14b)
                                    double eggCountFall = budwormCountFall * phenolLimit;

                                 
                                    // Reset site variables
                                    double currentHostFoliageFall = 0;
                                    double hostFoliageFall = 0;
                                    int siteTotalDefol = 0;

                                    double[] growthReductionList = new double[cohortList.Count];
                                    double[] pctMortalityList = new double[cohortList.Count];

                                    int cohortI = 0;
                                    foreach (Cohort cohort in cohortList)
                                    {
                                        totalFoliage = totalFoliageList[cohortI];  //TotalFoliage is an added cohort attribute
                                        currentFoliage = currentFoliageList[cohortI];  //CurrentFoliage is an added cohort attribute
                                        if (cohort.Biomass > 0)
                                            leafFraction = totalFoliage / cohort.Biomass;
                                        else
                                            leafFraction = 0;

                                        double percentMortality = 0;
                                        double growthReduction = 0;
                                        totalFoliageAfterDefol = totalFoliage;
                                        if (hostSpeciesList.Contains(cohort.Species) && (cohort.Age >= minSuscAge))
                                        {
                                            
                                            // assign defoliation to cohorts skewed by species and age (12a)
                                            double sppConvert = 0.0;
                                            if (cohort.Species.Name == "abiebals")
                                                sppConvert = 1.0;
                                            else if (cohort.Species.Name == "piceglau")
                                                sppConvert = 0.75;
                                            else if (cohort.Species.Name == "picemari")
                                                sppConvert = 0.375;
                                            double cohortDefol = pctDefol * sppConvert * cohort.Age / maxHostAge;
                                            if (cohortDefol > 100)
                                                cohortDefol = 100;
                                            defolHistList[cohortI][0] = (cohortDefol/100);  //DefolHist is an added cohort attribute
                                            int currentDefol = (int)Math.Round((currentFoliage * (cohortDefol/100)));  // Assumes all defoliation comes out of current year foliage, convert percentage to proportion
                                            siteTotalDefol += currentDefol;
                                            
                                                                                        
                                            

                                            // Calculate cumulative defoliation (15)
                                            // Cumulative Annual Weighted Defoliation (Hennigar)
                                            double annWtDefol0 = defolHistList[cohortI][0] * 0.28;
                                            double annWtDefol1 = defolHistList[cohortI][1] * 0.26;
                                            double annWtDefol2 = defolHistList[cohortI][2] * 0.22;
                                            double annWtDefol3 = defolHistList[cohortI][3] * 0.13;
                                            double annWtDefol4 = defolHistList[cohortI][4] * 0.08;
                                            double annWtDefol5 = defolHistList[cohortI][5] * 0.03;
                                            double cumAnnWtDefol = annWtDefol0 + annWtDefol1 + annWtDefol2 + annWtDefol3 + annWtDefol4 + annWtDefol5;

                                            // Calculate host tree impacts (16)
                                            // Growth reduction
                                            if (cbBudGR.Checked)
                                            {
                                                // Hennigar method
                                                growthReduction = 1 - (-0.0099 * (cumAnnWtDefol * 100) + 1.0182);
                                                
                                                // Dobesberger method
                                                //growthReduction = 0.339 * cumAnnWtDefol;

                                                if (growthReduction > 1.0)  // Cannot exceed 100%
                                                    growthReduction = 1.0;
                                                if (growthReduction < 0.0)  // Cannot be less than 0%
                                                    growthReduction = 0.0;
                                            }

                                            // Allocate impacts to cohorts (12a)
                                            // Biomass removal & mortality
                                            // Hennigar method
                                            const double CD_ScaleFactor = 0.952;
                                            double periodicDefoliation = cumAnnWtDefol * CD_ScaleFactor;
                                            // Apply budworm mortality
                                            // all cohorts treated equally
                                            if (cbBudmort.Checked)
                                            {
                                                percentMortality = Calculations.GetMortalityRate_AGE(cohort.Species, periodicDefoliation, cohort.Age);
                                            }
                                            int cohortMortality = (int)(percentMortality * (cohort.Biomass - currentDefol));
                                            totalSBWMortality += cohortMortality;
                                            int totalBiomassReduction = currentDefol + cohortMortality;
                                            cohort.ChangeBiomass(-1 * (int)totalBiomassReduction);
                                            if (cohort.Species == species1)
                                            {
                                                bioSum1 -= (int)totalBiomassReduction;
                                                newBioSum1 -= (int)totalBiomassReduction;
                                            }
                                            else if (cohort.Species == species2)
                                            {
                                                bioSum2 -= (int)totalBiomassReduction;
                                                newBioSum2 -= (int)totalBiomassReduction;
                                            }
                                            else if (cohort.Species == species3)
                                            {
                                                bioSum3 -= (int)totalBiomassReduction;
                                                newBioSum3 -= (int)totalBiomassReduction;
                                            }
                                            else if (cohort.Species == species4)
                                            {
                                                bioSum4 -= (int)totalBiomassReduction;
                                                newBioSum4 -= (int)totalBiomassReduction;
                                            }
                                            else if (cohort.Species == species5)
                                            {
                                                bioSum5 -= (int)totalBiomassReduction;
                                                newBioSum5 -= (int)totalBiomassReduction;
                                            }
                                            else if (cohort.Species == species6)
                                            {
                                                bioSum6 -= (int)totalBiomassReduction;
                                                newBioSum6 -= (int)totalBiomassReduction;
                                            }
                                            bioSumTotal -= (int)totalBiomassReduction;

                                            // Calculate remaining foliage (17)
                                            double currentFoliageAfterDefol = (currentFoliage - currentDefol)* (1-percentMortality);
                                            totalFoliageAfterDefol = (totalFoliage - currentDefol) * (1-percentMortality);
                                            // Tabulate site variable of remaining current host foliage
                                            currentHostFoliageFall += currentFoliageAfterDefol;
                                        }
                                        growthReductionList[cohortI] = growthReduction;
                                        pctMortalityList[cohortI] = percentMortality;
                                        cohortI++;
                                    }


                                    // Calculate SiteDefol (12a)
                                    double siteDefol = siteTotalDefol / currentHostFoliage;
                                    

                                    // reset site variables
                                    currentHostFoliage = 0;
                                    hostFoliage = 0;

                                    //Budworm - END - annual calculations


                                    // Start
                                    if ((rbAgeList.Checked) || (rbV30.Checked) || (rbV35.Checked))
                                    {

                                        List<int> groupList = new List<int>();
                                        int cohortIndex = -1;
                                        double groupBiomass = 0;
                                        double weightSum = 0;
                                        maxHostAge = 0;

                                        List<Cohort> updatedCohortList = new List<Cohort>(cohortList);
                                        foreach (Cohort cohort in cohortList)
                                        {
                                            double age = (double)cohort.Age;
                                            double shadeAge = age;
                                            double bio = (double)cohort.Biomass;
                                            //Budworm - read cohort attributes
                                            double growthReduction = growthReductionList[cohortIndex + 1];
                                            double percentMortality = pctMortalityList[cohortIndex + 1];
                                            if (bio > 0)
                                                leafFraction = totalFoliageList[cohortIndex + 1] / bio;
                                            else
                                                leafFraction = 0;

                                            // Assign cohorts to age class "groups" (meaning layers)

                                            if (rbV35.Checked)
                                            {
                                                weightSum = 0;
                                                foreach (Cohort xcohort in cohortList)
                                                {
                                                    double xmaxB = 0;
                                                    if (xcohort.Species == species1)
                                                    {
                                                        xmaxB = maxBiomass;
                                                    }
                                                    else if (xcohort.Species == species2)
                                                    {
                                                        xmaxB = maxBiomass2;
                                                    }
                                                    else if (xcohort.Species == species3)
                                                    {
                                                        xmaxB = maxBiomass3;
                                                    }
                                                    else if (xcohort.Species == species4)
                                                    {
                                                        xmaxB = maxBiomass4;
                                                    }
                                                    else if (xcohort.Species == species5)
                                                    {
                                                        xmaxB = maxBiomass5;
                                                    }
                                                    else if (xcohort.Species == species6)
                                                    {
                                                        xmaxB = maxBiomass6;
                                                    }
                                                    double potential = (xmaxB - siteBiomass + xcohort.Biomass);
                                                    /*  From BS 3.5 code - N/A here because no capacityReduction
                                                    //  Species can use new space from mortality immediately
                                                    //  but not in the case of capacity reduction due to harvesting.
                                                    if (capacityReduction >= 1.0)
                                                        potentialBiomass = Math.Max(potentialBiomass, SiteVars.PreviousYearMortality[site]);
                                                    */
                                                    double indexValue = 1.0;
                                                    if (potential > 0)
                                                        indexValue = Math.Min(1, (xcohort.Biomass / potential));
                                                    weightSum += (indexValue * xcohort.Biomass);
                                                }
                                                groupBiomass = siteBiomass;
                                            }

                                            cohortIndex += 1;

                                            int longevity = 0;
                                            double maxB = 0;
                                            double maxA = 0;
                                            double leafLong = 0;
                                            double paramK = 0;
                                            double maxLAI = 0;
                                            double paramD = 0;
                                            int shadeANPP = 0;
                                            double anppPower = 0;
                                            int shadeTol = 0;
                                            double maturityAge = 0;
                                            double decayRate = 0;
                                            int removeYear = -1;
                                            double removeProp = 0;
                                            bool removeBool = false;
                                            double scaleANPP = 1;
                                            int sppCount = 0;
                                            double X0 = 0;
                                            double mortR = 0;
                                            double thinCoeff = 1;
                                            double anppPeak = 1;
                                            double mortMod = 1;
                                            double resproutProb = 0.0;
                                            int minVegAge = 0;
                                            int maxVegAge = 0;
                                            double[] LAIArray = new double[simYears + 11];
                                            double[] CoBioArray = new double[simYears + 11];
                                            double[] ANPPArray = new double[simYears + 11];
                                            double[] MortAgeArray = new double[simYears + 11];
                                            double[] MortCompArray = new double[simYears + 11];
                                            //Budworm
                                            bool budwormHost = false;
                                            //bool budwormDecid = false;

                                            if (cohort.Species == species1)
                                            {
                                                longevity = longevity1;
                                                maxB = maxBiomass;
                                                maxA = maxANPP1;
                                                cohortCount1 += 1;
                                                leafLong = leafLongevity;
                                                paramK = paramK1;
                                                maxLAI = maxLAI1;
                                                paramD = paramD1;
                                                shadeANPP = shadeANPP1;
                                                anppPower = anppPower1;
                                                shadeTol = shadeTol1;
                                                maturityAge = maturityAge1;
                                                decayRate = decayRate1;
                                                removeYear = removeYear1;
                                                removeBool = removeBool1;
                                                removeProp = removeProp1;
                                                scaleANPP = scale1;
                                                spp1Count += 1;
                                                sppCount = spp1Count;
                                                X0 = X0_1;
                                                mortR = mortR_1;
                                                thinCoeff = thinCoeff1;
                                                anppPeak = anppPeak1;
                                                mortMod = mortMod1;
                                                resproutProb = resproutProb1;
                                                minVegAge = minVegAge1;
                                                maxVegAge = maxVegAge1;
                                                bioSum = bioSum1;
                                                removeSum = removeSum1;
                                                if (cohortCount1 <= coList1.Count)
                                                {
                                                    CoBioArray = coBioList1[cohortCount1 - 1];
                                                    LAIArray = coList1[cohortCount1 - 1];
                                                    ANPPArray = ANPPCoList1[cohortCount1 - 1];
                                                    MortAgeArray = MortAgeCoList1[cohortCount1 - 1];
                                                    MortCompArray = MortCompCoList1[cohortCount1 - 1];
                                                }
                                                else
                                                {
                                                    coBioList1.Add(CoBioArray);
                                                    coList1.Add(LAIArray);
                                                    ANPPCoList1.Add(ANPPArray);
                                                    MortAgeCoList1.Add(MortAgeArray);
                                                    MortCompCoList1.Add(MortCompArray);
                                                }
                                                //Budworm
                                                budwormHost = budwormHost1;

                                            }
                                            else if (cohort.Species == species2)
                                            {
                                                longevity = longevity2;
                                                maxB = maxBiomass2;
                                                maxA = maxANPP2;
                                                cohortCount2 += 1;
                                                leafLong = leafLongevity2;
                                                paramK = paramK2;
                                                maxLAI = maxLAI2;
                                                paramD = paramD2;
                                                shadeANPP = shadeANPP2;
                                                anppPower = anppPower2;
                                                shadeTol = shadeTol2;
                                                maturityAge = maturityAge2;
                                                decayRate = decayRate2;
                                                removeYear = removeYear2;
                                                removeBool = removeBool2;
                                                removeProp = removeProp2;
                                                scaleANPP = scale2;
                                                spp2Count += 1;
                                                sppCount = spp2Count;
                                                X0 = X0_2;
                                                mortR = mortR_2;
                                                thinCoeff = thinCoeff2;
                                                anppPeak = anppPeak2;
                                                mortMod = mortMod2;
                                                resproutProb = resproutProb2;
                                                minVegAge = minVegAge2;
                                                maxVegAge = maxVegAge2;
                                                bioSum = bioSum2;
                                                removeSum = removeSum2;
                                                if (cohortCount2 <= coList2.Count)
                                                {
                                                    CoBioArray = coBioList2[cohortCount2 - 1];
                                                    LAIArray = coList2[cohortCount2 - 1];
                                                    ANPPArray = ANPPCoList2[cohortCount2 - 1];
                                                    MortAgeArray = MortAgeCoList2[cohortCount2 - 1];
                                                    MortCompArray = MortCompCoList2[cohortCount2 - 1];
                                                }
                                                else
                                                {
                                                    coBioList2.Add(CoBioArray);
                                                    coList2.Add(LAIArray);
                                                    ANPPCoList2.Add(ANPPArray);
                                                    MortAgeCoList2.Add(MortAgeArray);
                                                    MortCompCoList2.Add(MortCompArray);
                                                }
                                                //Budworm
                                                budwormHost = budwormHost2;
                                            }
                                            else if (cohort.Species == species3)
                                            {
                                                longevity = longevity3;
                                                maxB = maxBiomass3;
                                                maxA = maxANPP3;
                                                cohortCount3 += 1;
                                                leafLong = leafLongevity3;
                                                paramK = paramK3;
                                                maxLAI = maxLAI3;
                                                paramD = paramD3;
                                                shadeANPP = shadeANPP3;
                                                anppPower = anppPower3;
                                                shadeTol = shadeTol3;
                                                maturityAge = maturityAge3;
                                                decayRate = decayRate3;
                                                removeYear = removeYear3;
                                                removeBool = removeBool3;
                                                removeProp = removeProp3;
                                                scaleANPP = scale3;
                                                spp3Count += 1;
                                                sppCount = spp3Count;
                                                X0 = X0_3;
                                                mortR = mortR_3;
                                                thinCoeff = thinCoeff3;
                                                anppPeak = anppPeak3;
                                                mortMod = mortMod3;
                                                resproutProb = resproutProb3;
                                                minVegAge = minVegAge3;
                                                maxVegAge = maxVegAge3;
                                                bioSum = bioSum3;
                                                removeSum = removeSum3;
                                                if (cohortCount3 <= coList3.Count)
                                                {
                                                    CoBioArray = coBioList3[cohortCount3 - 1];
                                                    LAIArray = coList3[cohortCount3 - 1];
                                                    ANPPArray = ANPPCoList3[cohortCount3 - 1];
                                                    MortAgeArray = MortAgeCoList3[cohortCount3 - 1];
                                                    MortCompArray = MortCompCoList3[cohortCount3 - 1];
                                                }
                                                else
                                                {
                                                    coBioList3.Add(CoBioArray);
                                                    coList3.Add(LAIArray);
                                                    ANPPCoList3.Add(ANPPArray);
                                                    MortAgeCoList3.Add(MortAgeArray);
                                                    MortCompCoList3.Add(MortCompArray);
                                                }
                                                //Budworm
                                                budwormHost = budwormHost3;
                                            }
                                            else if (cohort.Species == species4)
                                            {
                                                longevity = longevity4;
                                                maxB = maxBiomass4;
                                                maxA = maxANPP4;
                                                cohortCount4 += 1;
                                                leafLong = leafLongevity4;
                                                paramK = paramK4;
                                                maxLAI = maxLAI4;
                                                paramD = paramD4;
                                                shadeANPP = shadeANPP4;
                                                anppPower = anppPower4;
                                                shadeTol = shadeTol4;
                                                maturityAge = maturityAge4;
                                                decayRate = decayRate4;
                                                removeYear = removeYear4;
                                                removeBool = removeBool4;
                                                removeProp = removeProp4;
                                                scaleANPP = scale4;
                                                spp4Count += 1;
                                                sppCount = spp4Count;
                                                X0 = X0_4;
                                                mortR = mortR_4;
                                                thinCoeff = thinCoeff4;
                                                anppPeak = anppPeak4;
                                                mortMod = mortMod4;
                                                resproutProb = resproutProb4;
                                                minVegAge = minVegAge4;
                                                maxVegAge = maxVegAge4;
                                                bioSum = bioSum4;
                                                removeSum = removeSum4;
                                                if (cohortCount4 <= coList4.Count)
                                                {
                                                    CoBioArray = coBioList4[cohortCount4 - 1];
                                                    LAIArray = coList4[cohortCount4 - 1];
                                                    ANPPArray = ANPPCoList4[cohortCount4 - 1];
                                                    MortAgeArray = MortAgeCoList4[cohortCount4 - 1];
                                                    MortCompArray = MortCompCoList4[cohortCount4 - 1];
                                                }
                                                else
                                                {
                                                    coBioList4.Add(CoBioArray);
                                                    coList4.Add(LAIArray);
                                                    ANPPCoList4.Add(ANPPArray);
                                                    MortAgeCoList4.Add(MortAgeArray);
                                                    MortCompCoList4.Add(MortCompArray);
                                                }
                                                //Budworm
                                                budwormHost = budwormHost4;
                                            }
                                            else if (cohort.Species == species5)
                                            {
                                                longevity = longevity5;
                                                maxB = maxBiomass5;
                                                maxA = maxANPP5;
                                                cohortCount5 += 1;
                                                leafLong = leafLongevity5;
                                                paramK = paramK5;
                                                maxLAI = maxLAI5;
                                                paramD = paramD5;
                                                shadeANPP = shadeANPP5;
                                                anppPower = anppPower5;
                                                shadeTol = shadeTol5;
                                                maturityAge = maturityAge5;
                                                decayRate = decayRate5;
                                                removeYear = removeYear5;
                                                removeBool = removeBool5;
                                                removeProp = removeProp5;
                                                scaleANPP = scale5;
                                                spp5Count += 1;
                                                sppCount = spp5Count;
                                                X0 = X0_5;
                                                mortR = mortR_5;
                                                thinCoeff = thinCoeff5;
                                                anppPeak = anppPeak5;
                                                mortMod = mortMod5;
                                                resproutProb = resproutProb5;
                                                minVegAge = minVegAge5;
                                                maxVegAge = maxVegAge5;
                                                bioSum = bioSum5;
                                                removeSum = removeSum5;
                                                if (cohortCount5 <= coList5.Count)
                                                {
                                                    CoBioArray = coBioList5[cohortCount5 - 1];
                                                    LAIArray = coList5[cohortCount5 - 1];
                                                    ANPPArray = ANPPCoList5[cohortCount5 - 1];
                                                    MortAgeArray = MortAgeCoList5[cohortCount5 - 1];
                                                    MortCompArray = MortCompCoList5[cohortCount5 - 1];
                                                }
                                                else
                                                {
                                                    coBioList5.Add(CoBioArray);
                                                    coList5.Add(LAIArray);
                                                    ANPPCoList5.Add(ANPPArray);
                                                    MortAgeCoList5.Add(MortAgeArray);
                                                    MortCompCoList5.Add(MortCompArray);
                                                }
                                                //Budworm
                                                budwormHost = budwormHost5;
                                            }
                                            else if (cohort.Species == species6)
                                            {
                                                longevity = longevity6;
                                                maxB = maxBiomass6;
                                                maxA = maxANPP6;
                                                cohortCount6 += 1;
                                                leafLong = leafLongevity6;
                                                paramK = paramK6;
                                                maxLAI = maxLAI6;
                                                paramD = paramD6;
                                                shadeANPP = shadeANPP6;
                                                anppPower = anppPower6;
                                                shadeTol = shadeTol6;
                                                maturityAge = maturityAge6;
                                                decayRate = decayRate6;
                                                removeYear = removeYear6;
                                                removeBool = removeBool6;
                                                removeProp = removeProp6;
                                                scaleANPP = scale6;
                                                spp6Count += 1;
                                                sppCount = spp6Count;
                                                X0 = X0_6;
                                                mortR = mortR_6;
                                                thinCoeff = thinCoeff6;
                                                anppPeak = anppPeak6;
                                                mortMod = mortMod6;
                                                resproutProb = resproutProb6;
                                                minVegAge = minVegAge6;
                                                maxVegAge = maxVegAge6;
                                                bioSum = bioSum6;
                                                removeSum = removeSum6;
                                                if (cohortCount6 <= coList6.Count)
                                                {
                                                    CoBioArray = coBioList6[cohortCount6 - 1];
                                                    LAIArray = coList6[cohortCount6 - 1];
                                                    ANPPArray = ANPPCoList6[cohortCount6 - 1];
                                                    MortAgeArray = MortAgeCoList6[cohortCount6 - 1];
                                                    MortCompArray = MortCompCoList6[cohortCount6 - 1];
                                                }
                                                else
                                                {
                                                    coBioList6.Add(CoBioArray);
                                                    coList6.Add(LAIArray);
                                                    ANPPCoList6.Add(ANPPArray);
                                                    MortAgeCoList6.Add(MortAgeArray);
                                                    MortCompCoList6.Add(MortCompArray);
                                                }
                                                //Budworm
                                                budwormHost = budwormHost6;
                                            }
                                           
                                            // MAIN CALCULATIONS START HERE
                                            
                                            int totalRemove = 0;
                                            if (rbAllCohorts.Checked)
                                            {
                                                totalRemove = (int)(removeProp * bioSum);
                                            }

                                            // Age related mortality calculations

                                            double newBiomass = 0;
                                            double actualANPP = 0;
                                            int annualLeafANPP = 0;
                                            age += 1;
                                            if (rbAgeList.Checked)
                                            {
                                                newBiomass = 1;
                                                if (rbAllCohorts.Checked)
                                                {
                                                    if (((year + 1 - timestep) == removeYear) && (removeProp > 0))
                                                        removeBool = true;
                                                }
                                                else
                                                {
                                                    if (((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp > 0))
                                                        removeBool = true;
                                                }
                                            }
                                            else
                                            {
                                                double mortalityAgeProp = Calculations.ComputeAgeMortalityProp(X0, mortR, age);
                                                if (rbV30.Checked)
                                                    mortalityAgeProp = Calculations.ComputeAgeMortalityPropV2(paramD, longevity, age);
                                                double mortalityAge = bio * mortalityAgeProp * mortMod;
                                                if (mortalityAge > bio)
                                                    mortalityAge = bio;
                                                //  Potential biomass, equation 3 in Scheller and Mladenoff, 2004
                                                double potentialBiomass = Math.Max(0, maxB - siteBiomass + bio);

                                                // separating cohorts within a group (layer)
                                                double fractionB = bio / groupBiomass;

                                                double potMaxBio = maxB * fractionB;

                                                potentialBiomass = Math.Max(potentialBiomass, prevMort);
                                                double propANPP = 0;
                                                double potANPP = 0;
                                                double B_PM = 1;
                                                double B_AP = 1;


                                                // Calculation of B_AP & B_PM
                                                if (potentialBiomass > 0)
                                                {
                                                    if (rbV30.Checked)
                                                    {
                                                        //  Ratio of cohort's actual biomass to potential biomass
                                                        B_AP = bio / potentialBiomass;
                                                        // Modified to match B.S. 3.2 - should be changed back to cohortList - FIXME
                                                        B_PM = Calculations.CalculateCompetition(cohort, updatedCohortList);
                                                    }
                                                    else if ((rbV35.Checked))
                                                    {
                                                        //  Ratio of cohort's actual biomass to potential biomass
                                                        B_AP = bio / potentialBiomass;
                                                        //  Ratio of cohort's potential biomass to maximum biomass.  The
                                                        //  ratio cannot be exceed 1.
                                                        B_PM = Math.Min(1.0, potentialBiomass / maxB);
                                                    }
                                                }
                                                if (rbV35.Checked)
                                                {
                                                    // Matches BS 3.5
                                                    double weightedIndex = weightSum / siteBiomass;
                                                    B_AP = weightedIndex;
                                                    B_PM = 1.0;
                                                }

                                                if (rbV30.Checked || rbV35.Checked)
                                                {
                                                    // Proportion of maximum ANPP assigned to a cohort (main ANPP equation)
                                                    // Matches BS 3.5
                                                    propANPP = Calculations.ComputePropANPP(B_AP, B_PM, anppPower);

                                                    if (rbV35.Checked)
                                                    {

                                                        // Match BS 3.5
                                                        potANPP = propANPP * (maxA - (maxA * (1 / (1 + ((1 / X0) - 1) * Math.Exp(-1 * mortR * age)))));
                                                    }
                                                    else
                                                    {
                                                        potANPP = propANPP * maxA;
                                                    }

                                                    // Calculated actual ANPP can not exceed the limit set by the
                                                    //  maximum ANPP times the ratio of potential to maximum biomass.
                                                    //  This down regulates actual ANPP by the available growing space.
                                                    potANPP = Math.Min(maxA * B_PM, potANPP);
                                                }

                                                //Budworm
                                                // apply budworm growth reduction
                                                // all cohorts treated equally
                                                if(cbBudGR.Checked)
                                                    potANPP = potANPP - (growthReduction * potANPP);
                                                //Budworm - END - apply growth reduction
                                              
                                                actualANPP = Math.Max(1, potANPP);
                                                if (bio == 0)
                                                    actualANPP = 0;   

                                                double netANPP = actualANPP;
                                                if (rbV30.Checked)
                                                {
                                                    //  Age mortality is discounted from ANPP to prevent the over-
                                                    //  estimation of mortality.  ANPP cannot be negative.
                                                    netANPP = Math.Max(1, actualANPP - mortalityAge);
                                                    if (bio == 0)
                                                        netANPP = 0;
                                                }
                                                if (cbAgeMort.Checked)
                                                {
                                                    // Leaf portion of growth
                                                    annualLeafANPP = (int)Math.Round((netANPP * 0.35));  // Use constant 0.35 leaf fraction here
                                                }
                                                else
                                                {
                                                    // Leaf portion of growth
                                                    annualLeafANPP = (int)Math.Round((actualANPP * 0.35));  // Use constant 0.35 leaf fraction here
                                                }

                                                // Update Dead Biomass
                                                // From BS 3.5; N/A - Forest Floor not tracked here
                                                //ForestFloor.AddLitter(annualLeafANPP, species, site);

                                                //Budworm
                                                // calculate current host foliage (new leaf growth) (2)
                                                currentFoliageList[cohortIndex] = annualLeafANPP;
                                                if (budwormHost && (age >= minSuscAge))
                                                    currentHostFoliage += annualLeafANPP;
                                                //Budworm - END - calculate current host foliage

                                                double mortalityGrowth = 0;

                                                // Mortality Growth Equations
                                                if (rbV30.Checked)
                                                {
                                                    mortalityGrowth = Calculations.ComputeGrowthMortalityV3(netANPP, maxA, B_AP, B_PM, bio, mortalityAge, growthReduction);
                                                    
                                                }
                                                else if (rbV35.Checked)
                                                {
                                                    // Matches BS 3.5
                                                    mortalityGrowth = Calculations.ComputeGrowthMortality(netANPP, maxA, B_AP, 1.0, bio, mortalityAge, 1.0);

                                                    
                                                }
                                                //  Age-related mortality is discounted from growth-related
                                                //  mortality to prevent the under-estimation of mortality.  Cannot be negative.
                                                mortalityGrowth = Math.Max(0, mortalityGrowth - mortalityAge);
                                                //  Ensure that growth mortality does not exceed actualANPP.
                                                mortalityGrowth = Math.Min(mortalityGrowth, netANPP);
                                                //  Total mortality for the cohort
                                                double totalMortality = mortalityAge + mortalityGrowth;

                                                

                                                bool killed = false;
                                                if (rbAllCohorts.Checked)
                                                {
                                                    // If this is removal year for this spp, and totalRemoved has not been reached then additional biomass removed
                                                    if (((year + 1 - timestep) == removeYear) && (removeSum < totalRemove))
                                                    {
                                                        int thisRemove = totalRemove - removeSum;
                                                        if (thisRemove >= bio)
                                                        {
                                                            thisRemove = (int)bio;
                                                            killed = true;
                                                        }
                                                        totalMortality += thisRemove;
                                                        removeSum += thisRemove;
                                                    }
                                                }
                                                else
                                                {
                                                    // If this is removal year for this spp, and this is the oldest cohort of the spp then additional biomass removed
                                                    if (((year + 1 - timestep) == removeYear) && (sppCount == 1))
                                                        totalMortality = totalMortality + (bio * removeProp);
                                                }
                                                if (bio == 0)
                                                    killed = true;
                                                double defoliationLoss = 0.0; // From BS 3.5; N/A here
                                                // Matches BS 3.5
                                                int deltaBiomass = (int)(netANPP - totalMortality - defoliationLoss);

                                                // If reaches longevity, then removed
                                                if (age  >= cohort.Species.Longevity)
                                                    newBiomass = 0;
                                                else
                                                    newBiomass = Math.Max(0, bio + deltaBiomass);

                                                // Adjust if cohort completely removed
                                                if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0))||killed)
                                                {
                                                    totalMortality = bio;
                                                    newBiomass = 0;
                                                }

                                                //  Assume that the mortality is divided proportionally
                                                //  between the woody mass and non-woody mass (Niklaus & Enquist,
                                                //  2002).   Do not include current years growth.
                                                double mortality_nonwood = Math.Max(0.0, totalMortality * leafFraction);
                                                double mortality_wood = Math.Max(0.0, totalMortality - mortality_nonwood);

                                                //Budworm
                                                if (bio > 0)
                                                    totalFoliageAfterMort = totalFoliageAfterDefol - mortality_nonwood;
                                                else
                                                    totalFoliageAfterMort = 0;
                                                if (budwormHost && (age >= minSuscAge)) 
                                                    hostFoliageFall += totalFoliageAfterMort;


                                                // Add mortality to dead biomass pool (currentMort)
                                                currentMort += mortality_wood;
                                                if (deadWoodyBio + mortality_wood == 0)
                                                    totalDecayRate = 0;
                                                else
                                                    totalDecayRate = ((deadWoodyBio * totalDecayRate) + (mortality_wood * decayRate)) / (deadWoodyBio + mortality_wood);

                                                if ((year >= (timestep)) && (year <= (simYears + timestep)))
                                                {
                                                    ANPPArray[(year - (timestep))] = netANPP;
                                                    MortAgeArray[(year - (timestep))] = mortalityAge;
                                                    MortCompArray[(year - (timestep))] = mortalityGrowth;
                                                    CoBioArray[(year - (timestep))] = cohort.Biomass;
                                                }
                                                if (cohort.Species == species1)
                                                {
                                                    newBioSum1 -= bio;
                                                    removeSum1 = removeSum;
                                                    if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0))||killed)
                                                    {
                                                        newBioSum1 += 0;
                                                        removeBool1 = true;
                                                        removeBool = true;
                                                        if ((resproutProb > 0) && (age > minVegAge) && (age < maxVegAge))
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            if (checkRand < resproutProb)
                                                            {
                                                                resproutBool1 = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newBioSum1 += newBiomass;
                                                        removeBool = false;
                                                    }
                                                    coBioList1[cohortCount1 - 1] = CoBioArray;
                                                    ANPPCoList1[cohortCount1 - 1] = ANPPArray;
                                                    MortAgeCoList1[cohortCount1 - 1] = MortAgeArray;
                                                    MortCompCoList1[cohortCount1 - 1] = MortCompArray;
                                                }
                                                else if (cohort.Species == species2)
                                                {
                                                    newBioSum2 -= bio;
                                                    removeSum2 = removeSum;
                                                    if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0)) || killed)
                                                    {
                                                        newBioSum2 += 0;
                                                        removeBool2 = true;
                                                        removeBool = true;
                                                        if ((resproutProb > 0) && (age > minVegAge) && (age < maxVegAge))
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            if (checkRand < resproutProb)
                                                            {
                                                                resproutBool2 = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newBioSum2 += newBiomass;
                                                        removeBool = false;
                                                    }
                                                    coBioList2[cohortCount2 - 1] = CoBioArray;
                                                    ANPPCoList2[cohortCount2 - 1] = ANPPArray;
                                                    MortAgeCoList2[cohortCount2 - 1] = MortAgeArray;
                                                    MortCompCoList2[cohortCount2 - 1] = MortCompArray;
                                                }
                                                else if (cohort.Species == species3)
                                                {
                                                    newBioSum3 -= bio;
                                                    removeSum3 = removeSum;
                                                    if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0)) || killed)
                                                    {
                                                        newBioSum3 += 0;
                                                        removeBool3 = true;
                                                        removeBool = true;
                                                        if ((resproutProb > 0) && (age > minVegAge) && (age < maxVegAge))
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            if (checkRand < resproutProb)
                                                            {
                                                                resproutBool3 = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newBioSum3 += newBiomass;
                                                        removeBool = false;
                                                    }

                                                    coBioList3[cohortCount3 - 1] = CoBioArray;
                                                    ANPPCoList3[cohortCount3 - 1] = ANPPArray;
                                                    MortAgeCoList3[cohortCount3 - 1] = MortAgeArray;
                                                    MortCompCoList3[cohortCount3 - 1] = MortCompArray;
                                                }
                                                else if (cohort.Species == species4)
                                                {
                                                    newBioSum4 -= bio;
                                                    removeSum4 = removeSum;
                                                    if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0)) || killed)
                                                    {
                                                        newBioSum4 += 0;
                                                        removeBool4 = true;
                                                        removeBool = true;
                                                        if ((resproutProb > 0) && (age > minVegAge) && (age < maxVegAge))
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            if (checkRand < resproutProb)
                                                            {
                                                                resproutBool4 = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newBioSum4 += newBiomass;
                                                        removeBool = false;
                                                    }

                                                    coBioList4[cohortCount4 - 1] = CoBioArray;
                                                    ANPPCoList4[cohortCount4 - 1] = ANPPArray;
                                                    MortAgeCoList4[cohortCount4 - 1] = MortAgeArray;
                                                    MortCompCoList4[cohortCount4 - 1] = MortCompArray;
                                                }
                                                else if (cohort.Species == species5)
                                                {
                                                    newBioSum5 -= bio;
                                                    removeSum5 = removeSum;
                                                    if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0)) || killed)
                                                    {
                                                        newBioSum5 += 0;
                                                        removeBool5 = true;
                                                        removeBool = true;
                                                        if ((resproutProb > 0) && (age > minVegAge) && (age < maxVegAge))
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            if (checkRand < resproutProb)
                                                            {
                                                                resproutBool5 = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newBioSum5 += newBiomass;
                                                        removeBool = false;
                                                    }

                                                    coBioList5[cohortCount5 - 1] = CoBioArray;
                                                    ANPPCoList5[cohortCount5 - 1] = ANPPArray;
                                                    MortAgeCoList5[cohortCount5 - 1] = MortAgeArray;
                                                    MortCompCoList5[cohortCount5 - 1] = MortCompArray;
                                                }
                                                else if (cohort.Species == species6)
                                                {
                                                    newBioSum6 -= bio;
                                                    removeSum6 = removeSum;
                                                    if ((((year + 1 - timestep) == removeYear) && (sppCount == 1) && (removeProp == 1.0)) || killed)
                                                    {
                                                        newBioSum6 += 0;
                                                        removeBool6 = true;
                                                        removeBool = true;
                                                        if ((resproutProb > 0) && (age > minVegAge) && (age < maxVegAge))
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            if (checkRand < resproutProb)
                                                            {
                                                                resproutBool6 = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        newBioSum6 += newBiomass;
                                                        removeBool = false;
                                                    }

                                                    coBioList6[cohortCount6 - 1] = CoBioArray;
                                                    ANPPCoList6[cohortCount6 - 1] = ANPPArray;
                                                    MortAgeCoList6[cohortCount6 - 1] = MortAgeArray;
                                                    MortCompCoList6[cohortCount6 - 1] = MortCompArray;
                                                }
                                            }
                                            if (cohort.Age + 1 < cohort.Species.Longevity)
                                            {
                                                if (removeBool == false)
                                                {

                                                    Cohort newCohort = new Cohort(cohort.Species, (ushort)(cohort.Age + 1), (int)newBiomass);
                                                    newCohortList.Add(newCohort);
                                                    // Added to match Biomass Succession v3.2 - should be removed - FIXME
                                                    updatedCohortList[cohortIndex] = newCohort;

                                                    if ((cohort.Age) > maturityAge)
                                                    {
                                                        shadeClass = (byte)Math.Max(shadeClass, shadeTol);
                                                    }
                                                    //Budworm
                                                    double[] oldDefolHist = defolHistList[cohortIndex];
                                                    double[] newDefolHist = new double[10];
                                                    for (int i = 0; i < (oldDefolHist.Length - 1); i++)
                                                    {
                                                        newDefolHist[i + 1] = oldDefolHist[i];
                                                    }
                                                    newDefolHist[0] = 0;
                                                    newDefolHistList.Add(newDefolHist);
                                                    int newTotalFoliage = (int)(annualLeafANPP + totalFoliageAfterDefol * (1 - (1 / leafLong)));
                                                    newTotalFoliageList.Add(newTotalFoliage);
                                                    newCurrentFoliageList.Add(annualLeafANPP);
                                                    if (budwormHost && ((cohort.Age+1) >= minSuscAge))
                                                    {
                                                        hostFoliage += newTotalFoliage;
                                                        maxHostAge = Math.Max(maxHostAge, cohort.Age + 1);
                                                    //double woodBio = newBiomass - totalFoliage;
                                                    //newWoodList.Add(woodBio);
                                                    }

                                                }
                                            }

                                            if (newBiomass > maxCohortBio)
                                            {
                                                cohort1Bio = newBiomass;
                                                if (maxCohortBio > secondCohortBio)
                                                {
                                                    cohort2Bio = maxCohortBio;
                                                    secondCohortBio = maxCohortBio;
                                                }
                                                maxCohortBio = newBiomass;
                                            }
                                            else if (newBiomass > secondCohortBio)
                                            {
                                                cohort2Bio = newBiomass;
                                                secondCohortBio = newBiomass;
                                            }


                                            lastGroupShadeTol = shadeTol;
                                            lastGroupIndex = groupIndex;
                                        }

                                        deadWoodyBio = (deadWoodyBio + currentMort) * Math.Exp(-1.0 * totalDecayRate);

                                        if ((rbV30.Checked) || (rbV35.Checked))
                                        {
                                            double B_AM = (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / (double)maxShadeBiomass;

                                            shadeClass = 0;
                                            if (B_AM >= (shade1 / 100))
                                                shadeClass = 1;
                                            if (B_AM >= (shade2 / 100))
                                                shadeClass = 2;
                                            if (B_AM >= (shade3 / 100))
                                                shadeClass = 3;
                                            if (B_AM >= (shade4 / 100))
                                                shadeClass = 4;
                                            if (B_AM >= (shade5 / 100))
                                                shadeClass = 5;
                                            pctShade = Math.Min(1.0, B_AM);
                                        }

                                    }
                                    

                                    if (resproutBool1)
                                    {
                                        bool canEstablish = false;
                                        if (rbAgeList.Checked)
                                        {
                                            if (shadeTol1 == 5)
                                            {
                                                if (shadeClass > 1)
                                                    canEstablish = true;
                                            }
                                            else
                                                if (shadeClass < shadeTol1)
                                                    canEstablish = true;

                                        }
                                        else
                                        {
                                                canEstablish = true;
                                        }
                                        if (canEstablish)
                                        {
                                            double myRand = random.Next(0, 1000000);
                                            double checkRand = myRand / 1000000;
                                            double suffLightMod = 1.0;
                                            if (rbV30.Checked || rbV35.Checked)
                                            {
                                                List<double> lightList = sufficientLight[shadeTol1 - 1];
                                                suffLightMod = lightList[(int)shadeClass];
                                            }
                                            if (checkRand < suffLightMod)
                                            {
                                                if (rbAgeList.Checked)
                                                {
                                                    initBiomass = 1;
                                                }
                                                else// if (rbV35.Checked)
                                                {
                                                    initBiomass = maxANPP1 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass = Math.Min(maxANPP1, initBiomass);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass = Math.Max(1.0, initBiomass);
                                                }
                                                /*else
                                                {
                                                    initBiomass = 0.025 * maxBiomass * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass = Math.Min(maxANPP1, initBiomass);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass = Math.Max(1.0, initBiomass);
                                                 
                                                }*/
                                                Cohort newCohort = new Cohort(species1, 1, (int)initBiomass);
                                                newCohortList.Add(newCohort);
                                                //Budworm
                                                currentFoliage = (int)initBiomass * 0.35;
                                                if (budwormHost1 && (1 >= minSuscAge))
                                                {
                                                    currentHostFoliage += currentFoliage;
                                                    hostFoliage += currentFoliage;
                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                }
                                                double[] newDefolHist = new double[10];
                                                newDefolHistList.Add(newDefolHist);
                                                newTotalFoliageList.Add((int)currentFoliage);
                                                newCurrentFoliageList.Add((int)currentFoliage);
                                             
                                                //newWoodList.Add(initBiomass - currentFoliage);
                                            }
                                        }
                                    }
                                    if (resproutBool2)
                                    {
                                        bool canEstablish = false;
                                        if (rbAgeList.Checked)
                                        {
                                            if (shadeTol2 == 5)
                                            {
                                                if (shadeClass > 1)
                                                    canEstablish = true;
                                            }
                                            else
                                                if (shadeClass < shadeTol2)
                                                    canEstablish = true;
                                        }
                                        else
                                        {
                                            canEstablish = true;
                                        }
   
                                        if (canEstablish)
                                        {
                                            double myRand = random.Next(0, 1000000);
                                            double checkRand = myRand / 1000000;
                                            double suffLightMod = 1.0;
                                            if (rbV30.Checked || rbV35.Checked)
                                            {
                                                List<double> lightList = sufficientLight[shadeTol2 - 1];
                                                suffLightMod = lightList[(int)shadeClass];
                                            }
                                            if (checkRand < suffLightMod)
                                            {
                                                if (rbAgeList.Checked)
                                                {
                                                    initBiomass2 = 1;
                                                }
                                                else// if (rbV35.Checked)
                                                {
                                                    initBiomass2 = maxANPP2 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass2 = Math.Min(maxANPP2, initBiomass2);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass2 = Math.Max(1.0, initBiomass2);
                                                }
                                                /*else
                                                {
                                                    initBiomass2 = 0.025 * maxBiomass2 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass2 = Math.Min(maxANPP2, initBiomass2);

                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass2 = Math.Max(1.0, initBiomass2);
                                                }*/
                                                Cohort newCohort = new Cohort(species2, 1, (int)initBiomass2);
                                                newCohortList.Add(newCohort);
                                                //Budworm
                                                currentFoliage = (int)initBiomass2 * 0.35;
                                                if (budwormHost2 && (1 >= minSuscAge))
                                                {
                                                    currentHostFoliage += currentFoliage;
                                                    hostFoliage += currentFoliage;
                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                }
                                                double[] newDefolHist = new double[10];
                                                newDefolHistList.Add(newDefolHist);
                                                newTotalFoliageList.Add((int)currentFoliage);
                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                //newWoodList.Add(initBiomass - currentFoliage);
                                            }
                                        }
                                    }
                                    if (resproutBool3)
                                    {
                                        bool canEstablish = false;
                                        if (rbAgeList.Checked)
                                        {
                                            if (shadeTol3 == 5)
                                            {
                                                if (shadeClass > 1)
                                                    canEstablish = true;
                                            }
                                            else
                                                if (shadeClass < shadeTol3)
                                                    canEstablish = true;

                                        }
                                        else
                                        {
                                                canEstablish = true;
                                        }
                                        if (canEstablish)
                                        {
                                            double myRand = random.Next(0, 1000000);
                                            double checkRand = myRand / 1000000;
                                            double suffLightMod = 1.0;
                                            if (rbV30.Checked || rbV35.Checked)
                                            {
                                                List<double> lightList = sufficientLight[shadeTol3 - 1];
                                                suffLightMod = lightList[(int)shadeClass];
                                            }
                                            if (checkRand < suffLightMod)
                                            {
                                                if (rbAgeList.Checked)
                                                {
                                                    initBiomass3 = 1;
                                                }
                                                else// if (rbV35.Checked)
                                                {
                                                    initBiomass3 = maxANPP3 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass3 = Math.Min(maxANPP3, initBiomass3);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass3 = Math.Max(1.0, initBiomass3);
                                                }
                                                /*else
                                                {
                                                    initBiomass3 = 0.025 * maxBiomass3 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass3 = Math.Min(maxANPP3, initBiomass3);

                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass3 = Math.Max(1.0, initBiomass3);
                                                }*/
                                                Cohort newCohort = new Cohort(species3, 1, (int)initBiomass3);
                                                newCohortList.Add(newCohort);
                                                //Budworm
                                                currentFoliage = (int)initBiomass3 * 0.35;
                                                if (budwormHost3 && (1 >= minSuscAge))
                                                {
                                                    currentHostFoliage += currentFoliage;
                                                    hostFoliage += currentFoliage;
                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                }
                                                double[] newDefolHist = new double[10];
                                                newDefolHistList.Add(newDefolHist);
                                                newTotalFoliageList.Add((int)currentFoliage);
                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                //newWoodList.Add(initBiomass - currentFoliage);
                                            }
                                        }
                                    }
                                    if (resproutBool4)
                                    {
                                        bool canEstablish = false;
                                        if (rbAgeList.Checked)
                                        {
                                            if (shadeTol4 == 5)
                                            {
                                                if (shadeClass > 1)
                                                    canEstablish = true;
                                            }
                                            else
                                                if (shadeClass < shadeTol4)
                                                    canEstablish = true;

                                        }
                                        else
                                        {
                                                canEstablish = true;
                                        }
                                        if (canEstablish)
                                        {
                                            double myRand = random.Next(0, 1000000);
                                            double checkRand = myRand / 1000000;
                                            double suffLightMod = 1.0;
                                            if (rbV30.Checked || rbV35.Checked)
                                            {
                                                List<double> lightList = sufficientLight[shadeTol4 - 1];
                                                suffLightMod = lightList[(int)shadeClass];
                                            }
                                            if (checkRand < suffLightMod)
                                            {
                                                if (rbAgeList.Checked)
                                                {
                                                    initBiomass4 = 1;
                                                }
                                                else// if (rbV35.Checked)
                                                {
                                                    initBiomass4 = maxANPP4 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass4 = Math.Min(maxANPP4, initBiomass4);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass4 = Math.Max(1.0, initBiomass4);
                                                }
                                                /*else
                                                {
                                                    initBiomass4 = 0.025 * maxBiomass4 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass4 = Math.Min(maxANPP4, initBiomass4);

                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass4 = Math.Max(1.0, initBiomass4);
                                                }*/
                                                Cohort newCohort = new Cohort(species4, 1, (int)initBiomass4);
                                                newCohortList.Add(newCohort);
                                                //Budworm
                                                currentFoliage = (int)initBiomass4 * 0.35;
                                                if (budwormHost4 && (1 >= minSuscAge))
                                                {
                                                    currentHostFoliage += currentFoliage;
                                                    hostFoliage += currentFoliage;
                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                }
                                                double[] newDefolHist = new double[10];
                                                newDefolHistList.Add(newDefolHist);
                                                newTotalFoliageList.Add((int)currentFoliage);
                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                //newWoodList.Add(initBiomass - currentFoliage);
                                            }
                                        }
                                    }
                                    if (resproutBool5)
                                    {
                                        bool canEstablish = false;
                                        if (rbAgeList.Checked)
                                        {
                                            if (shadeTol5 == 5)
                                            {
                                                if (shadeClass > 1)
                                                    canEstablish = true;
                                            }
                                            else
                                                if (shadeClass < shadeTol5)
                                                    canEstablish = true;

                                        }
                                        else
                                        {
                                            canEstablish = true;
                                        }
                                        if (canEstablish)
                                        {
                                            double myRand = random.Next(0, 1000000);
                                            double checkRand = myRand / 1000000;
                                            double suffLightMod = 1.0;
                                            if (rbV30.Checked || rbV35.Checked)
                                            {
                                                List<double> lightList = sufficientLight[shadeTol5 - 1];
                                                suffLightMod = lightList[(int)shadeClass];
                                            }
                                            if (checkRand < suffLightMod)
                                            {
                                                if (rbAgeList.Checked)
                                                {
                                                    initBiomass5 = 1;
                                                }
                                                else// if (rbV35.Checked)
                                                {
                                                    initBiomass5 = maxANPP5 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass5 = Math.Min(maxANPP5, initBiomass5);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass5 = Math.Max(1.0, initBiomass5);
                                                }
                                                /*else
                                                {
                                                    initBiomass5 = 0.025 * maxBiomass5 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass5 = Math.Min(maxANPP5, initBiomass5);

                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass5 = Math.Max(1.0, initBiomass5);
                                                }*/
                                                Cohort newCohort = new Cohort(species5, 1, (int)initBiomass5);
                                                newCohortList.Add(newCohort);
                                                //Budworm
                                                currentFoliage = (int)initBiomass5 * 0.35;
                                                if (budwormHost5 && (1 >= minSuscAge))
                                                {
                                                    currentHostFoliage += currentFoliage;
                                                    hostFoliage += currentFoliage;
                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                }
                                                double[] newDefolHist = new double[10];
                                                newDefolHistList.Add(newDefolHist);
                                                newTotalFoliageList.Add((int)currentFoliage);
                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                //newWoodList.Add(initBiomass - currentFoliage);
                                            }
                                        }
                                    }
                                    if (resproutBool6)
                                    {
                                        bool canEstablish = false;
                                        if (rbAgeList.Checked)
                                        {
                                            if (shadeTol6 == 5)
                                            {
                                                if (shadeClass > 1)
                                                    canEstablish = true;
                                            }
                                            else
                                                if (shadeClass < shadeTol6)
                                                    canEstablish = true;

                                        }
                                        else
                                        {
                                            canEstablish = true;
                                        }
                                        if (canEstablish)
                                        {
                                            double myRand = random.Next(0, 1000000);
                                            double checkRand = myRand / 1000000;
                                            double suffLightMod = 1.0;
                                            if (rbV30.Checked || rbV35.Checked)
                                            {
                                                List<double> lightList = sufficientLight[shadeTol6 - 1];
                                                suffLightMod = lightList[(int)shadeClass];
                                            }
                                            if (checkRand < suffLightMod)
                                            {
                                                if (rbAgeList.Checked)
                                                {
                                                    initBiomass6 = 1;
                                                }
                                                else// if (rbV35.Checked)
                                                {
                                                    initBiomass6 = maxANPP6 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass6 = Math.Min(maxANPP6, initBiomass6);
                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass6 = Math.Max(1.0, initBiomass6);
                                                }
                                                /*else
                                                {
                                                    initBiomass6 = 0.025 * maxBiomass6 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                    // Initial biomass cannot be greater than maxANPP
                                                    initBiomass6 = Math.Min(maxANPP6, initBiomass6);

                                                    //  Initial biomass cannot be less than 1.
                                                    initBiomass6 = Math.Max(1.0, initBiomass6);
                                                }*/
                                                Cohort newCohort = new Cohort(species6, 1, (int)initBiomass6);
                                                newCohortList.Add(newCohort);
                                                //Budworm
                                                currentFoliage = (int)initBiomass6 * 0.35;
                                                if (budwormHost6 && (1 >= minSuscAge))
                                                {
                                                    currentHostFoliage += currentFoliage;
                                                    hostFoliage += currentFoliage;
                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                }
                                                double[] newDefolHist = new double[10];
                                                newDefolHistList.Add(newDefolHist);
                                                newTotalFoliageList.Add((int)currentFoliage);
                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                //newWoodList.Add(initBiomass - currentFoliage);
                                            }
                                        }
                                    }
                                    if (!resproutBool1 && !resproutBool2 && !resproutBool3 && !resproutBool4 && !resproutBool5 && !resproutBool6)
                                    {
                                        if (((year + 1) % timestep == 0))
                                        {
                                            bool canEstablish = false;
                                            if (plantYear1 == (year + 1 - timestep))
                                                canEstablish = true;
                                            else if (rbAgeList.Checked)
                                            {
                                                if (shadeTol1 == 5)
                                                {
                                                    if (shadeClass > 1)
                                                        canEstablish = true;
                                                }
                                                else
                                                    if (shadeClass < shadeTol1)
                                                        canEstablish = true;

                                            }
                                            else 
                                            {
                                                    canEstablish = true;
                                            }

                                            if (canEstablish)
                                            {
                                                if (((year) >= seedYear1) || ((plantYear1 == (year + 1 - timestep))))
                                                {
                                                    if (!spp1Renew)
                                                    {
                                                        double myRand = random.Next(0, 1000000);
                                                        double checkRand = myRand / 1000000;
                                                        double suffLightMod = 1.0;
                                                        if (rbV30.Checked || rbV35.Checked)
                                                        {
                                                            List<double> lightList = sufficientLight[shadeTol1 - 1];
                                                            suffLightMod = lightList[(int)shadeClass];
                                                        }
                                                        double sppEstabMod = sppEstab * suffLightMod;
                                                        if ((checkRand < sppEstabMod) || ((plantYear1 == (year + 1 - timestep))))
                                                        {
                                                            if (rbAgeList.Checked)
                                                            {
                                                                initBiomass = 1;
                                                            }
                                                            else// if (rbV35.Checked)
                                                            {
                                                                initBiomass = maxANPP1 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                // Initial biomass cannot be greater than maxANPP
                                                                initBiomass = Math.Min(maxANPP1, initBiomass);
                                                                //  Initial biomass cannot be less than 1.
                                                                initBiomass = Math.Max(1.0, initBiomass);
                                                            }
                                                            /*else
                                                            {
                                                                initBiomass = 0.025 * maxBiomass * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                // Initial biomass cannot be greater than maxANPP
                                                                initBiomass = Math.Min(maxANPP1, initBiomass);
                                                                //  Initial biomass cannot be less than 1.
                                                                initBiomass = Math.Max(1.0, initBiomass);
                                                            }*/
                                                            Cohort newCohort = new Cohort(species1, 1, (int)initBiomass);
                                                            newCohortList.Add(newCohort);
                                                            //Budworm
                                                            currentFoliage = (int)initBiomass * 0.35;
                                                            if (budwormHost1 && (1 >= minSuscAge))
                                                            {
                                                                currentHostFoliage += currentFoliage;
                                                                hostFoliage += currentFoliage;
                                                                maxHostAge = Math.Max(maxHostAge, 1);
                                                            }
                                                            double[] newDefolHist = new double[10];
                                                            newDefolHistList.Add(newDefolHist);
                                                            newTotalFoliageList.Add((int)currentFoliage);
                                                            newCurrentFoliageList.Add((int)currentFoliage);
                                                            //newWoodList.Add(initBiomass - currentFoliage);

                                                        }
                                                    }
                                                }

                                            }
                                            if (numSpecies > 1)
                                            {
                                                canEstablish = false;
                                                if (plantYear2 == (year + 1 - timestep))
                                                    canEstablish = true;
                                                else if (rbAgeList.Checked)
                                                {
                                                    if (shadeTol2 == 5)
                                                    {
                                                        if (shadeClass > 1)
                                                            canEstablish = true;
                                                    }
                                                    else
                                                        if (shadeClass < shadeTol2)
                                                            canEstablish = true;

                                                }
                                                else
                                                {
                                                        canEstablish = true;
                                                }
                                                if (canEstablish)
                                                {
                                                    if (((year) >= seedYear2) || ((plantYear2 == (year + 1 - timestep))))
                                                    {
                                                        if (!spp2Renew)
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            double suffLightMod = 1.0;
                                                            if (rbV30.Checked || rbV35.Checked)
                                                            {
                                                                List<double> lightList = sufficientLight[shadeTol2 - 1];
                                                                suffLightMod = lightList[(int)shadeClass];
                                                            }
                                                            double sppEstabMod = sppEstab2 * suffLightMod;
                                                            if ((checkRand < sppEstabMod) || ((plantYear2 == (year - timestep + 1))))
                                                            {
                                                                if (rbAgeList.Checked)
                                                                {
                                                                    initBiomass2 = 1;
                                                                }
                                                                else// if (rbV35.Checked)
                                                                {
                                                                    initBiomass2 = maxANPP2 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass2 = Math.Min(maxANPP2, initBiomass2);
                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass2 = Math.Max(1.0, initBiomass2);
                                                                }
                                                                /*else
                                                                {
                                                                    initBiomass2 = 0.025 * maxBiomass2 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass2 = Math.Min(maxANPP2, initBiomass2);

                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass2 = Math.Max(1.0, initBiomass2);
                                                                }*/
                                                                Cohort newCohort = new Cohort(species2, 1, (int)initBiomass2);
                                                                newCohortList.Add(newCohort);
                                                                //Budworm
                                                                currentFoliage = (int)initBiomass2 * 0.35;
                                                                if (budwormHost2 && (1 >= minSuscAge))
                                                                {
                                                                    currentHostFoliage += currentFoliage;
                                                                    hostFoliage += currentFoliage;
                                                                }
                                                                double[] newDefolHist = new double[10];
                                                                newDefolHistList.Add(newDefolHist);
                                                                newTotalFoliageList.Add((int)currentFoliage);
                                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                                //newWoodList.Add(initBiomass - currentFoliage);

                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            if (numSpecies > 2)
                                            {
                                                canEstablish = false;
                                                if (plantYear3 == (year + 1 - timestep))
                                                    canEstablish = true;
                                                else if (rbAgeList.Checked)
                                                {
                                                    if (shadeTol3 == 5)
                                                    {
                                                        if (shadeClass > 1)
                                                            canEstablish = true;
                                                    }
                                                    else
                                                        if (shadeClass < shadeTol3)
                                                            canEstablish = true;

                                                }
                                                else 
                                                {
                                                        canEstablish = true;
                                                }
                                                if (canEstablish)
                                                {
                                                    if (((year) >= seedYear3) || ((plantYear3 == (year + 1 - timestep))))
                                                    {
                                                        if (!spp3Renew)
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            double suffLightMod = 1.0;
                                                            if (rbV30.Checked || rbV35.Checked)
                                                            {
                                                                List<double> lightList = sufficientLight[shadeTol3 - 1];
                                                                suffLightMod = lightList[(int)shadeClass];
                                                            }
                                                            double sppEstabMod = sppEstab3 * suffLightMod;
                                                            if ((checkRand < sppEstabMod) || ((plantYear3 == (year - timestep + 1))))
                                                            {
                                                                if (rbAgeList.Checked)
                                                                {
                                                                    initBiomass3 = 1;
                                                                }
                                                                else// if (rbV35.Checked)
                                                                {
                                                                    initBiomass3 = maxANPP3 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass3 = Math.Min(maxANPP3, initBiomass3);
                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass3 = Math.Max(1.0, initBiomass3);
                                                                }
                                                                /*else
                                                                {
                                                                    initBiomass3 = 0.025 * maxBiomass3 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass3 = Math.Min(maxANPP3, initBiomass3);

                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass3 = Math.Max(1.0, initBiomass3);
                                                                }*/
                                                                Cohort newCohort = new Cohort(species3, 1, (int)initBiomass3);
                                                                newCohortList.Add(newCohort);
                                                                //Budworm
                                                                currentFoliage = (int)initBiomass3 * 0.35;
                                                                if (budwormHost3 && (1 >= minSuscAge))
                                                                {
                                                                    currentHostFoliage += currentFoliage;
                                                                    hostFoliage += currentFoliage;
                                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                                }
                                                                double[] newDefolHist = new double[10];
                                                                newDefolHistList.Add(newDefolHist);
                                                                newTotalFoliageList.Add((int)currentFoliage);
                                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                                //newWoodList.Add(initBiomass - currentFoliage);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            if (numSpecies > 3)
                                            {
                                                canEstablish = false;
                                                if (plantYear4 == (year + 1 - timestep))
                                                    canEstablish = true;
                                                else if (rbAgeList.Checked)
                                                {
                                                    if (shadeTol4 == 5)
                                                    {
                                                        if (shadeClass > 1)
                                                            canEstablish = true;
                                                    }
                                                    else
                                                        if (shadeClass < shadeTol4)
                                                            canEstablish = true;

                                                }
                                                else 
                                                {
                                                        canEstablish = true;
                                                }
                                                if (canEstablish)
                                                {
                                                    if (((year) >= seedYear4) || ((plantYear4 == (year + 1 - timestep))))
                                                    {
                                                        if (!spp4Renew)
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            double suffLightMod = 1.0;
                                                            if (rbV30.Checked || rbV35.Checked)
                                                            {
                                                                List<double> lightList = sufficientLight[shadeTol4 - 1];
                                                                suffLightMod = lightList[(int)shadeClass];
                                                            }
                                                            double sppEstabMod = sppEstab4 * suffLightMod;
                                                            if ((checkRand < sppEstabMod) || ((plantYear4 == (year - timestep + 1))))
                                                            {
                                                                if (rbAgeList.Checked)
                                                                {
                                                                    initBiomass4 = 1;
                                                                }
                                                                else// if (rbV35.Checked)
                                                                {
                                                                    initBiomass4 = maxANPP4 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass4 = Math.Min(maxANPP4, initBiomass4);
                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass4 = Math.Max(1.0, initBiomass4);
                                                                }
                                                                /*else
                                                                {
                                                                    initBiomass4 = 0.025 * maxBiomass4 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass4 = Math.Min(maxANPP4, initBiomass4);

                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass4 = Math.Max(1.0, initBiomass4);
                                                                }*/
                                                                Cohort newCohort = new Cohort(species4, 1, (int)initBiomass4);
                                                                newCohortList.Add(newCohort);
                                                                //Budworm
                                                                currentFoliage = (int)initBiomass4 * 0.35;
                                                                if (budwormHost4 && (1 >= minSuscAge))
                                                                {
                                                                    currentHostFoliage += currentFoliage;
                                                                    hostFoliage += currentFoliage;
                                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                                }
                                                                double[] newDefolHist = new double[10];
                                                                newDefolHistList.Add(newDefolHist);
                                                                newTotalFoliageList.Add((int)currentFoliage);
                                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                                //newWoodList.Add(initBiomass - currentFoliage);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            if (numSpecies > 4)
                                            {
                                                canEstablish = false;
                                                if (plantYear5 == (year + 1 - timestep))
                                                    canEstablish = true;
                                                else if (rbAgeList.Checked)
                                                {
                                                    if (shadeTol5 == 5)
                                                    {
                                                        if (shadeClass > 1)
                                                            canEstablish = true;
                                                    }
                                                    else
                                                        if (shadeClass < shadeTol5)
                                                            canEstablish = true;

                                                }
                                                else
                                                {
                                                    canEstablish = true;
                                                }
                                                if (canEstablish)
                                                {
                                                    if (((year) >= seedYear5) || ((plantYear5 == (year + 1 - timestep))))
                                                    {
                                                        if (!spp5Renew)
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            double suffLightMod = 1.0;
                                                            if (rbV30.Checked || rbV35.Checked)
                                                            {
                                                                List<double> lightList = sufficientLight[shadeTol5 - 1];
                                                                suffLightMod = lightList[(int)shadeClass];
                                                            }
                                                            double sppEstabMod = sppEstab5 * suffLightMod;
                                                            if ((checkRand < sppEstabMod) || ((plantYear5 == (year - timestep + 1))))
                                                            {
                                                                if (rbAgeList.Checked)
                                                                {
                                                                    initBiomass5 = 1;
                                                                }
                                                                else// if (rbV35.Checked)
                                                                {
                                                                    initBiomass5 = maxANPP5 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass5 = Math.Min(maxANPP5, initBiomass5);
                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass5 = Math.Max(1.0, initBiomass5);
                                                                }
                                                                /*else
                                                                {
                                                                    initBiomass5 = 0.025 * maxBiomass5 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass5 = Math.Min(maxANPP5, initBiomass5);

                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass5 = Math.Max(1.0, initBiomass5);
                                                                }*/
                                                                Cohort newCohort = new Cohort(species5, 1, (int)initBiomass5);
                                                                newCohortList.Add(newCohort);
                                                                //Budworm
                                                                currentFoliage = (int)initBiomass5 * 0.35;
                                                                if (budwormHost5 && (1 >= minSuscAge))
                                                                {
                                                                    currentHostFoliage += currentFoliage;
                                                                    hostFoliage += currentFoliage;
                                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                                }
                                                                double[] newDefolHist = new double[10];
                                                                newDefolHistList.Add(newDefolHist);
                                                                newTotalFoliageList.Add((int)currentFoliage);
                                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                                //newWoodList.Add(initBiomass - currentFoliage);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            if (numSpecies > 5)
                                            {
                                                canEstablish = false;
                                                if (plantYear6 == (year + 1 - timestep))
                                                    canEstablish = true;
                                                else if (rbAgeList.Checked)
                                                {
                                                    if (shadeTol6 == 5)
                                                    {
                                                        if (shadeClass > 1)
                                                            canEstablish = true;
                                                    }
                                                    else
                                                        if (shadeClass < shadeTol6)
                                                            canEstablish = true;

                                                }
                                                else
                                                {
                                                    canEstablish = true;
                                                }
                                                if (canEstablish)
                                                {
                                                    if (((year) >= seedYear6) || ((plantYear6 == (year + 1 - timestep))))
                                                    {
                                                        if (!spp6Renew)
                                                        {
                                                            double myRand = random.Next(0, 1000000);
                                                            double checkRand = myRand / 1000000;
                                                            double suffLightMod = 1.0;
                                                            if (rbV30.Checked || rbV35.Checked)
                                                            {
                                                                List<double> lightList = sufficientLight[shadeTol6 - 1];
                                                                suffLightMod = lightList[(int)shadeClass];
                                                            }
                                                            double sppEstabMod = sppEstab6 * suffLightMod;
                                                            if ((checkRand < sppEstabMod) || ((plantYear6 == (year - timestep + 1))))
                                                            {
                                                                if (rbAgeList.Checked)
                                                                {
                                                                    initBiomass6 = 1;
                                                                }
                                                                else// if (rbV35.Checked)
                                                                {
                                                                    initBiomass6 = maxANPP6 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass6 = Math.Min(maxANPP6, initBiomass6);
                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass6 = Math.Max(1.0, initBiomass6);
                                                                }
                                                                /*else
                                                                {
                                                                    initBiomass6 = 0.025 * maxBiomass6 * Math.Exp(-1.6 * (newBioSum1 + newBioSum2 + newBioSum3 + newBioSum4 + newBioSum5 + newBioSum6) / siteMaxBio);
                                                                    // Initial biomass cannot be greater than maxANPP
                                                                    initBiomass6 = Math.Min(maxANPP6, initBiomass6);

                                                                    //  Initial biomass cannot be less than 1.
                                                                    initBiomass6 = Math.Max(1.0, initBiomass6);
                                                                }*/
                                                                Cohort newCohort = new Cohort(species6, 1, (int)initBiomass6);
                                                                newCohortList.Add(newCohort);
                                                                //Budworm
                                                                currentFoliage = (int)initBiomass6 * 0.35;
                                                                if (budwormHost6 && (1 >= minSuscAge))
                                                                {
                                                                    currentHostFoliage += currentFoliage;
                                                                    hostFoliage += currentFoliage;
                                                                    maxHostAge = Math.Max(maxHostAge, 1);
                                                                }
                                                                double[] newDefolHist = new double[10];
                                                                newDefolHistList.Add(newDefolHist);
                                                                newTotalFoliageList.Add((int)currentFoliage);
                                                                newCurrentFoliageList.Add((int)currentFoliage);
                                                                //newWoodList.Add(initBiomass - currentFoliage);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    

                                    //First years up to timestep are spin-up (not recorded)
                                    if ((year >= (timestep)) && (year <= (simYears + timestep)))
                                    {                                       
                                        shadeClassArray[(year - (timestep))] = shadeClass;
                                        numCohortsArray[(year - (timestep))] = cohortCount1;
                                        numCohortsArray2[(year - (timestep))] = cohortCount2;
                                        numCohortsArray3[(year - (timestep))] = cohortCount3;
                                        numCohortsArray4[(year - (timestep))] = cohortCount4;
                                        numCohortsArray5[(year - (timestep))] = cohortCount5;
                                        numCohortsArray6[(year - (timestep))] = cohortCount6;
                                        bioSumArray1[(year - (timestep))] = bioSum1;
                                        bioSumArray2[(year - (timestep))] = bioSum2;
                                        bioSumArray3[(year - (timestep))] = bioSum3;
                                        bioSumArray4[(year - (timestep))] = bioSum4;
                                        bioSumArray5[(year - (timestep))] = bioSum5;
                                        bioSumArray6[(year - (timestep))] = bioSum6;
                                        cohortArray1[(year - (timestep))] = cohort1Bio;
                                        cohortArray2[(year - (timestep))] = cohort2Bio;
                                        deadWoodArray[(year - (timestep))] = deadWoodyBio;
                                        //Budworm - add values to arrays
                                        budwormDensArray[(year - (timestep))] = filteredDensitySpring;
                                        budwormCountArray[(year - (timestep))] = budwormCountSpring;
                                        budEnemyDensArray[(year - (timestep))] = enemyDensitySpring_xt;
                                        budEnemyCountArray[(year - (timestep))] = enemyCountWinter;
                                        pctDefoliationArray[(year - (timestep))] = (pctDefol/100);
                                        siteDefoliationArray[(year - (timestep))] = siteDefol;
                                        hostFoliageArray[(year - (timestep))] = hostFoliage;
                                        currentHostFoliageArray[(year - (timestep))] = currentHostFoliage;
                                        hostFoliageFallArray[(year - (timestep))] = hostFoliageFall;
                                        currentHostFoliageFallArray[(year - (timestep))] = currentHostFoliageFall;
                                        budwormMortalityArray[(year - (timestep))] = totalSBWMortality;
                                        //Budworm - END - add values to arrays

                                    }
                                    if ((year >= (timestep - 1)) && (year <= (simYears + timestep - 1)))
                                    {
                                        pctShadeArray[(year - (timestep - 1))] = pctShade * 100;
                                    }
                                    prevMort = currentMort;
                                    bioSum1 = newBioSum1 + (int)initBiomass;
                                    bioSum2 = newBioSum2 + (int)initBiomass2;
                                    bioSum3 = newBioSum3 + (int)initBiomass3;
                                    bioSum4 = newBioSum4 + (int)initBiomass4;
                                    bioSum5 = newBioSum5 + (int)initBiomass5;
                                    bioSum6 = newBioSum6 + (int)initBiomass6;
                                    bioSumTotal = bioSum1 + bioSum2 + bioSum3 + bioSum4 + bioSum5 + bioSum6;

                                    cohortList = newCohortList;
                                    //Budworm
                                    // Add Emigration and Immigration functions
                                    // Calculate LDD/SDD ratio (18)
                                    double LDDHabitat = 0;
                                    double LDDFlight = 0;
                                    double LDDRatio = 0;
                                    if (cbEmigration.Checked)
                                    {
                                        double m = 0;
                                        double b = 1;
                                        if (siteDefol < minLDD)
                                            b = 0;
                                        else if ((siteDefol > minLDD) && (siteDefol <= halfLDD))
                                        {
                                            if ((halfLDD - minLDD) > 0)
                                            {
                                                m = 0.5 / (halfLDD - minLDD);
                                            }
                                            b = 0.5 - (m * halfLDD);
                                        }
                                        else if ((siteDefol > halfLDD) && (siteDefol <= maxLDD))
                                        {
                                            if ((maxLDD - halfLDD) > 0)
                                            {
                                                m = 0.5 / (maxLDD - halfLDD);
                                            }
                                            b = 1.0 - (m * maxLDD);
                                        }
                                        LDDHabitat = m * siteDefol + b;  //confirmed with spreadsheet
                                        //if (siteDefol > maxLDD)
                                        //    LDDHabitat = 1.0;

                                        double slope = (maxLDDProp - (1 - maxLDDProp)) / (1.0 - 0.46);  //confirmed with spreadsheet
                                        double intercept = maxLDDProp - slope;  //confirmed with spreadsheet
                                        if (cbPositiveFecund.Checked)
                                        {
                                            if (rprimeZ < 0.46)
                                                LDDFlight = 0.0;
                                            else
                                                LDDFlight = slope * rprimeZ + intercept;
                                        }
                                        else  // confirmed with spreadsheet
                                        {
                                            double slope2 = (-1.0) * slope;
                                            double intercept2 = (-1.0) * intercept + 1.0;
                                            if (rprimeZ < 0.46)
                                                LDDFlight = maxLDDProp;
                                            else
                                                LDDFlight = slope2 * rprimeZ + intercept2;
                                        }
                                        LDDRatio = LDDHabitat * LDDFlight;
                                    }
                                    // Calculate LDD dispersers (18b)
                                    double LDDout = eggCountFall *  LDDRatio;
                                    // Calculate SDD dispersers (18b)
                                    // for site model assume SDD stay on site
                                    double SDDout = 0;
                                    // Distribute LDD dispersers (19)
                                    // for site model assume no immigrants
                                    double LDDin = 0;
                                    // Distribute SDD dispersers (20)
                                    // for site model assume emigrants = immigrants
                                    double SDDin = 0;

                                     //First years up to timestep are spin-up (not recorded)
                                    if ((year >= (timestep)) && (year <= (simYears + timestep)))
                                    {
                                        emigrationArray[(year - (timestep))] = LDDout + SDDout - LDDin - SDDin;
                                    }
                                    budwormCount = eggCountFall - LDDout - SDDout + LDDin + SDDin;


                                    defolHistList = newDefolHistList;
                                    currentFoliageList = newCurrentFoliageList;
                                    totalFoliageList = newTotalFoliageList;
                                    //woodList = newWoodList;

                                    lastGroupIndex = groupIndex;

                                }

                                if (rbV30.Checked || rbV35.Checked)
                                {
                                    if (numSpecies == 6)
                                    {
                                        GraphPane pane1 = CreateGraph(graph1, "Biomass", bioSumArray1, bioSumArray2, bioSumArray3, bioSumArray4, bioSumArray5, bioSumArray6, c);
                                        pane1.XAxis.Scale.Max = simYears;
                                        pane1.YAxis.Title.Text = "Biomass (g/m2)";
                                    }
                                    if (numSpecies == 5)
                                    {
                                        GraphPane pane1 = CreateGraph(graph1, "Biomass", bioSumArray1, bioSumArray2, bioSumArray3, bioSumArray4, bioSumArray5, c);
                                        pane1.XAxis.Scale.Max = simYears;
                                        pane1.YAxis.Title.Text = "Biomass (g/m2)";
                                    }
                                    if (numSpecies == 4)
                                    {
                                        GraphPane pane1 = CreateGraph(graph1, "Biomass", bioSumArray1, bioSumArray2, bioSumArray3, bioSumArray4, c);
                                        pane1.XAxis.Scale.Max = simYears;
                                        pane1.YAxis.Title.Text = "Biomass (g/m2)";
                                    }
                                    else if (numSpecies == 3)
                                    {
                                        GraphPane pane1 = CreateGraph(graph1, "Biomass", bioSumArray1, bioSumArray2, bioSumArray3, c);
                                        pane1.XAxis.Scale.Max = simYears;
                                        pane1.YAxis.Title.Text = "Biomass (g/m2)";
                                    }
                                    else if (numSpecies == 2)
                                    {
                                        GraphPane pane1 = CreateGraph(graph1, "Biomass", bioSumArray1, bioSumArray2, c);
                                        pane1.XAxis.Scale.Max = simYears;
                                        pane1.YAxis.Title.Text = "Biomass (g/m2)";
                                    }
                                    else
                                    {
                                        GraphPane pane1 = CreateGraph(graph1, "Biomass", bioSumArray1, c);
                                        pane1.XAxis.Scale.Max = simYears;
                                        pane1.YAxis.Title.Text = "Biomass (g/m2)";
                                    }

                                    GraphPane pane2 = CreateGraph(graph2, "Percent Shade", pctShadeArray, c);
                                    pane2.YAxis.Scale.Max = 100;
                                    pane2.XAxis.Scale.Max = simYears;

                                    GraphPane pane6 = CreateGraph(graph6, "Cohort ANPP", ANPPCoList, c);
                                    pane6.XAxis.Scale.Max = simYears;
                                    pane6.YAxis.Title.Text = "ANPP (g/m2)";

                                    GraphPane pane9 = CreateGraph(graph9, "Cohort Biomass", coBioList, c);
                                    pane9.XAxis.Scale.Max = simYears;
                                    pane9.YAxis.Title.Text = "Biomass (g/m2)";

                                    GraphPane pane10 = CreateGraph(graph10, "Dead Woody Biomass", deadWoodArray, c);
                                    pane10.XAxis.Scale.Max = simYears;
                                    pane10.YAxis.Title.Text = "Biomass (g/m2)";

                                    graph1.Refresh();
                                    graph2.Refresh();
                                    graph6.Refresh();
                                    graph9.Refresh();
                                    graph10.Refresh();
                                }

                                GraphPane pane3 = CreateGraph(graph3, "Shade Class", shadeClassArray, c);
                                pane3.YAxis.Scale.Max = 5;
                                pane3.YAxis.Scale.MajorStep = 1;
                                pane3.XAxis.Scale.Max = simYears;
                                
                                if (numSpecies == 6)
                                {
                                    GraphPane pane4 = CreateGraph(graph4, "Number of Cohorts", numCohortsArray, numCohortsArray2, numCohortsArray3, numCohortsArray4, numCohortsArray5, numCohortsArray6, c);
                                    pane4.XAxis.Scale.Max = simYears;
                                    pane4.YAxis.Scale.MajorStep = 1;
                                }
                                if (numSpecies == 5)
                                {
                                    GraphPane pane4 = CreateGraph(graph4, "Number of Cohorts", numCohortsArray, numCohortsArray2, numCohortsArray3, numCohortsArray4, numCohortsArray5, c);
                                    pane4.XAxis.Scale.Max = simYears;
                                    pane4.YAxis.Scale.MajorStep = 1;
                                } 
                                if (numSpecies == 4)
                                {
                                    GraphPane pane4 = CreateGraph(graph4, "Number of Cohorts", numCohortsArray, numCohortsArray2, numCohortsArray3, numCohortsArray4, c);
                                    pane4.XAxis.Scale.Max = simYears;
                                    pane4.YAxis.Scale.MajorStep = 1;
                                } 
                                else if (numSpecies == 3)
                                {
                                    GraphPane pane4 = CreateGraph(graph4, "Number of Cohorts", numCohortsArray, numCohortsArray2, numCohortsArray3, c);
                                    pane4.XAxis.Scale.Max = simYears;
                                    pane4.YAxis.Scale.MajorStep = 1;
                                }
                                else if (numSpecies == 2)
                                {
                                    GraphPane pane4 = CreateGraph(graph4, "Number of Cohorts", numCohortsArray, numCohortsArray2, c);
                                    pane4.XAxis.Scale.Max = simYears;
                                    pane4.YAxis.Scale.MajorStep = 1;
                                }
                                else
                                {
                                    GraphPane pane4 = CreateGraph(graph4, "Number of Cohorts", numCohortsArray, c);
                                    pane4.XAxis.Scale.Max = simYears;
                                    pane4.YAxis.Scale.MajorStep = 1;
                                }
                                
                                graph3.Refresh();
                                graph4.Refresh();

                                //Budworm - Graph outputs
                                // graph of budworm density
                                GraphPane pane5 = CreateGraph(graph_BudwormD, "Budworm Density", budwormDensArray, c);
                                pane5.YAxis.Type = AxisType.Linear;
                                pane5.YAxis.Scale.Min = 0;
                                pane5.YAxis.Title.Text = "Budworm Density (#/g current host foliage)";
                                //pane5.YAxis.Scale.MajorStep = 1;
                                //pane5.YAxis.Type = AxisType.Log;
                                pane5.XAxis.Scale.Max = simYears;
                                graph_BudwormD.AxisChange();
                                graph_BudwormD.Refresh();

                                // graph of budworm count
                                GraphPane pane7 = CreateGraph(graph_BudwormC, "Budworm Count", budwormCountArray, c);
                                pane7.YAxis.Title.Text = "Budworm Count (#/m2)";
                                //pane7.YAxis.Scale.Max = 10;
                                //pane7.YAxis.Scale.MajorStep = 1;
                                pane7.XAxis.Scale.Max = simYears;
                                graph_BudwormC.Refresh();

                                // graph of enemy density
                                GraphPane pane8 = CreateGraph(graph_EnemyDens, "Enemy Density", budEnemyDensArray, c);
                                pane8.YAxis.Type = AxisType.Linear;
                                pane8.YAxis.Scale.Min = 0;
                                pane8.YAxis.Title.Text = "Enemy Density (#/L2 budworm)";
                                //pane8.YAxis.Scale.MajorStep = 1;
                                //pane8.YAxis.Type = AxisType.Log;
                                pane8.XAxis.Scale.Max = simYears;
                                graph_EnemyDens.AxisChange();
                                graph_EnemyDens.Refresh();

                                // graph of enemy count
                                GraphPane pane11 = CreateGraph(graph_EnemyCount, "Enemy Count", budEnemyCountArray, c);
                                pane11.YAxis.Title.Text = "Enemy Count (#/m2)";
                                //pane11.YAxis.Scale.Max = 10;
                                //pane11.YAxis.Scale.MajorStep = 1;
                                pane11.XAxis.Scale.Max = simYears;
                                graph_EnemyCount.Refresh();

                                // graph of foliage
                                GraphPane pane12 = CreateGraph(graph_HostFoliage, "Current Host Foliage", currentHostFoliageArray, c);
                                pane12.YAxis.Title.Text = "Host Foliage (g/m2)";
                                //pane12.YAxis.Scale.Max = 10;
                                //pane12.YAxis.Scale.MajorStep = 1;
                                pane12.XAxis.Scale.Max = simYears;
                                graph_HostFoliage.Refresh();

                                // graph of prop defoliation
                                GraphPane pane13 = CreateGraph(graph_PropDefol, "Proportion Defoliation", pctDefoliationArray, c);
                                pane13.YAxis.Scale.Max = 1;
                                //pane13.YAxis.Scale.MajorStep = 1;
                                pane13.XAxis.Scale.Max = simYears;
                                graph_PropDefol.Refresh();

                                // graph of budworm density
                                GraphPane pane14 = CreateGraph(graph_LogPopDens, "Log Population Density", budwormDensArray,budEnemyDensArray, c);
                                pane14.YAxis.Type = AxisType.Log;
                                pane14.AxisChange();
                                //pane14.YAxis.Scale.Min = Math.Min(Calculations.MinNonZeroInArray(budwormDensArray), Calculations.MinNonZeroInArray(budEnemyDensArray));
                                pane14.YAxis.Scale.Min = 0.000000001;
                                pane14.YAxis.Title.Text = "Population Density (budworm & enemy)";
                                pane14.XAxis.Scale.Max = simYears;
                                //graph_LogPopDens.AxisChange();
                                graph_LogPopDens.Refresh();

                                // graph of SBW mortality
                                GraphPane pane15 = CreateGraph(graph_SBWmortality, "SBW Mortality", budwormMortalityArray, c);
                                pane15.XAxis.Scale.Max = simYears;
                                pane15.YAxis.Title.Text = "Mortality Biomass (g/m2)";
                                graph_SBWmortality.Refresh();

                                // graph of SBW net migration
                                GraphPane pane16 = CreateGraph(graph_SBWmigration, "SBW Migration", emigrationArray, c);
                                pane16.XAxis.Scale.Max = simYears;
                                pane16.YAxis.Title.Text = "Net Migration (count)";
                                graph_SBWmigration.Refresh();

                                //Budworm - END - budworm graphs

                                if (checkBoxLog.Checked)
                                {
                                    //Create output log textfile
                                    StringBuilder sb = new StringBuilder();
                                    if (numSpecies == 6)
                                    {
                                        string[] header = new string[] { "Year", "Biomass1", "Biomass2", "Biomass3", "Biomass4", "Biomass5", "Biomass6", "PctShade", "ShadeClass", "NumCohorts1", "NumCohorts2", "NumCohorts3", "NumCohorts4", "NumCohorts5", "NumCohorts6", "DeadWoodyBio", "BudwormDensity", "BudwormCount", "EnemyDensity", "EnemyCount", "HostFoliage", "CurrentHostFoliage","PropDefoliation" };
                                        sb.AppendLine(string.Join(",", header));
                                        for (int index = 0; index < bioSumArray1.Length; index++)
                                        {
                                            string[] output = new string[]{
                                            index.ToString(),
                                            bioSumArray1[index].ToString(),
                                            bioSumArray2[index].ToString(),
                                            bioSumArray3[index].ToString(),
                                            bioSumArray4[index].ToString(),                            
                                            bioSumArray5[index].ToString(),                           
                                            bioSumArray6[index].ToString(),
                                            pctShadeArray[index].ToString(),
                                            shadeClassArray[index].ToString(),
                                            numCohortsArray[index].ToString(),
                                            numCohortsArray2[index].ToString(),
                                            numCohortsArray3[index].ToString(),
                                            numCohortsArray4[index].ToString(),
                                            numCohortsArray5[index].ToString(),
                                            numCohortsArray6[index].ToString(),
                                            deadWoodArray[index].ToString(),
                                            budwormDensArray[index].ToString(),
                                            budwormCountArray[index].ToString(),
                                            budEnemyDensArray[index].ToString(),
                                            budEnemyCountArray[index].ToString(),
                                            hostFoliageArray[index].ToString(),
                                            currentHostFoliageArray[index].ToString(),
                                            pctDefoliationArray[index].ToString()
                                            };
                                            sb.AppendLine(string.Join(",", output));
                                        }

                                    }
                                    else if (numSpecies == 5)
                                    {
                                        string[] header = new string[] { "Year", "Biomass1", "Biomass2", "Biomass3", "Biomass4", "Biomass5", "PctShade", "ShadeClass", "NumCohorts1", "NumCohorts2", "NumCohorts3", "NumCohorts4", "NumCohorts5", "DeadWoodyBio", "BudwormDensity", "BudwormCount", "EnemyDensity", "EnemyCount", "HostFoliage", "CurrentHostFoliage", "PropDefoliation" };
                                        sb.AppendLine(string.Join(",", header));
                                        for (int index = 0; index < bioSumArray1.Length; index++)
                                        {
                                            string[] output = new string[]{
                            index.ToString(),
                            bioSumArray1[index].ToString(),
                            bioSumArray2[index].ToString(),
                            bioSumArray3[index].ToString(),
                            bioSumArray4[index].ToString(),                            
                            bioSumArray5[index].ToString(),
                            pctShadeArray[index].ToString(),
                            shadeClassArray[index].ToString(),
                            numCohortsArray[index].ToString(),
                            numCohortsArray2[index].ToString(),
                            numCohortsArray3[index].ToString(),
                            numCohortsArray4[index].ToString(),
                            numCohortsArray5[index].ToString(),
                            deadWoodArray[index].ToString(),
                                            budwormDensArray[index].ToString(),
                                            budwormCountArray[index].ToString(),
                                            budEnemyDensArray[index].ToString(),
                                            budEnemyCountArray[index].ToString(),
                                            hostFoliageArray[index].ToString(),
                                            currentHostFoliageArray[index].ToString(),
                                            pctDefoliationArray[index].ToString()};
                                            sb.AppendLine(string.Join(",", output));
                                        }

                                    }
                                    else if (numSpecies == 4)
                                    {
                                        string[] header = new string[] { "Year", "Biomass1", "Biomass2", "Biomass3", "Biomass4", "PctShade", "ShadeClass", "NumCohorts1", "NumCohorts2", "NumCohorts3", "NumCohorts4", "DeadWoodyBio", "BudwormDensity", "BudwormCount", "EnemyDensity", "EnemyCount", "HostFoliage", "CurrentHostFoliage", "PropDefoliation" };
                                        sb.AppendLine(string.Join(",", header));
                                        for (int index = 0; index < bioSumArray1.Length; index++)
                                        {
                                            string[] output = new string[]{
                            index.ToString(),
                            bioSumArray1[index].ToString(),
                            bioSumArray2[index].ToString(),
                            bioSumArray3[index].ToString(),
                            bioSumArray4[index].ToString(),
                            pctShadeArray[index].ToString(),
                            shadeClassArray[index].ToString(),
                            numCohortsArray[index].ToString(),
                            numCohortsArray2[index].ToString(),
                            numCohortsArray3[index].ToString(),
                            numCohortsArray4[index].ToString(),
                            deadWoodArray[index].ToString(),
                                            budwormDensArray[index].ToString(),
                                            budwormCountArray[index].ToString(),
                                            budEnemyDensArray[index].ToString(),
                                            budEnemyCountArray[index].ToString(),
                                            hostFoliageArray[index].ToString(),
                                            currentHostFoliageArray[index].ToString(),
                                            pctDefoliationArray[index].ToString()};
                                            sb.AppendLine(string.Join(",", output));
                                        }

                                    }
                                    else if (numSpecies == 3)
                                    {
                                        string[] header = new string[] { "Year", "Biomass1", "Biomass2", "Biomass3", "PctShade", "ShadeClass", "NumCohorts1", "NumCohorts2", "NumCohorts3", "DeadWoodyBio", "BudwormDensity", "BudwormCount", "EnemyDensity", "EnemyCount", "HostFoliage", "CurrentHostFoliage", "PropDefoliation" };
                                        sb.AppendLine(string.Join(",", header));
                                        for (int index = 0; index < bioSumArray1.Length; index++)
                                        {
                                            string[] output = new string[]{
                            index.ToString(),
                            bioSumArray1[index].ToString(),
                            bioSumArray2[index].ToString(),
                            bioSumArray3[index].ToString(),
                            pctShadeArray[index].ToString(),
                            shadeClassArray[index].ToString(),
                            numCohortsArray[index].ToString(),
                            numCohortsArray2[index].ToString(),
                            numCohortsArray3[index].ToString(),
                            deadWoodArray[index].ToString(),
                                            budwormDensArray[index].ToString(),
                                            budwormCountArray[index].ToString(),
                                            budEnemyDensArray[index].ToString(),
                                            budEnemyCountArray[index].ToString(),
                                            hostFoliageArray[index].ToString(),
                                            currentHostFoliageArray[index].ToString(),
                                            pctDefoliationArray[index].ToString()};
                                            sb.AppendLine(string.Join(",", output));
                                        }

                                    }
                                    else if (numSpecies == 2)
                                    {
                                        string[] header = new string[] { "Year", "Biomass1", "Biomass2", "PctShade", "ShadeClass", "NumCohorts1", "NumCohorts2", "DeadWoodyBio", "BudwormDensity", "BudwormCount", "EnemyDensity", "EnemyCount", "HostFoliage", "CurrentHostFoliage", "PropDefoliation" };
                                        sb.AppendLine(string.Join(",", header));
                                        for (int index = 0; index < bioSumArray1.Length; index++)
                                        {
                                            string[] output = new string[]{
                            index.ToString(),
                            bioSumArray1[index].ToString(),
                            bioSumArray2[index].ToString(),
                            pctShadeArray[index].ToString(),
                            shadeClassArray[index].ToString(),
                            numCohortsArray[index].ToString(),
                            numCohortsArray2[index].ToString(),
                            deadWoodArray[index].ToString(),
                                            budwormDensArray[index].ToString(),
                                            budwormCountArray[index].ToString(),
                                            budEnemyDensArray[index].ToString(),
                                            budEnemyCountArray[index].ToString(),
                                            hostFoliageArray[index].ToString(),
                                            currentHostFoliageArray[index].ToString(),
                                            pctDefoliationArray[index].ToString()};
                                            sb.AppendLine(string.Join(",", output));
                                        }

                                    }
                                    else
                                    {
                                        string[] header = new string[] { "Year", "Biomass", "PctShade", "ShadeClass", "NumCohorts", "Cohort1Bio", "Cohort1LAI", "Cohort2Bio", "DeadWoodyBio", "BudwormDensity", "BudwormCount", "EnemyDensity", "EnemyCount", "HostFoliage", "CurrentHostFoliage", "PropDefoliation" };

                                        sb.AppendLine(string.Join(",", header));
                                        for (int index = 0; index < bioSumArray1.Length; index++)
                                        {
                                            string[] output = new string[]{
                                    index.ToString(),
                                    bioSumArray1[index].ToString(),
                                    pctShadeArray[index].ToString(),
                                    numCohortsArray[index].ToString(),
                                    shadeClassArray[index].ToString(),
                                    cohortArray1[index].ToString(),
                                    LAIArray1[index].ToString(),
                                    cohortArray2[index].ToString(),
                                    deadWoodArray[index].ToString(),
                                            budwormDensArray[index].ToString(),
                                            budwormCountArray[index].ToString(),
                                            budEnemyDensArray[index].ToString(),
                                            budEnemyCountArray[index].ToString(),
                                            hostFoliageArray[index].ToString(),
                                            currentHostFoliageArray[index].ToString(),
                                            pctDefoliationArray[index].ToString()};
                                            sb.AppendLine(string.Join(",", output));
                                        }
                                    }
                                    File.WriteAllText(outTextFile, sb.ToString());

                                    //Create budworm parameter log textfile
                                    StringBuilder paramString = new StringBuilder();
                                    string[] paramHeader = new string[] { "rm", "r'm", "b", "b'", "c'", "c", "a'", "InitEnemyDens", "InitBudDens", "OverwinterMean", "OverwinterStd", "PredationMean", "PredationStd", "Mating_a", "Mating_b", "DecidProtect", "PhenolLimitMean", "PhenolLimitStd", "FoodLimit_b''", "FoodLimit_a''", "FoodLimit_Deltaa''", "FoodLimit_Deltab''","GrowthReduction","Mortality" };
                                    paramString.AppendLine(string.Join(",", paramHeader));
                                    string[] paramValues = new string[]{budwormrm.ToString(),
                                        budwormrprimem.ToString(),
                                        budwormb.ToString(),budwormbprime.ToString(),
                                        budwormcprime.ToString(),
                                        budwormc.ToString(),
                                        budwormaprime.ToString(),
                                        budEnemyDensity0.ToString(),
                                        budwormDensity0.ToString(),
                                        budwormWinterMean.ToString(),
                                        budwormWinterStdev.ToString(),
                                        budwormPredationMean.ToString(),
                                        budwormPredationStdev.ToString(),
                                        matingA.ToString(),
                                        matingB.ToString(),
                                        decidProtectDmax2.ToString(),
                                        phenolLimitMean.ToString(),
                                        phenolLimitStdev.ToString(),
                                        budwormbprime2.ToString(),
                                        budwormaprime2.ToString(),
                                        budwormdeltaaprime2.ToString(),
                                        budwormdeltabprime2.ToString(),
                                        cbBudGR.Checked.ToString(),
                                        cbBudmort.Checked.ToString()};
                                   paramString.AppendLine(string.Join(",",paramValues));
                                   File.WriteAllText(paramTextFile, paramString.ToString());
                                }
                            }
                            rangeCount++;
                        }
                    }
            }
            

        }




        private void buttonCancel_Click(object sender, EventArgs e)
        {
            repCount = 1;
            this.Close();
        }

        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, Array myArray, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();

            int index = 0;
            foreach (double bio in myArray)
            {
                list.Add(index, bio);
                index++;
            }
            string myLabel = (repCount-1).ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            LineItem curve = myPane.AddCurve(myLabel, list, c, SymbolType.None);
         
            curve.Line.Width = 2;
            if (menuBatchMode.Checked)
                curve.Line.Width = 1;

            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }

        private GraphPane CreateGraphPoints(ZedGraphControl zgc, string yLabel, Dictionary<int,double> myArray)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();

            
            foreach (KeyValuePair<int,double> pair in myArray)
            {
                int age = pair.Key;
                double bio = pair.Value;
                
                list.Add(age, bio);
                
            }
            string myLabel = "Reference";

            LineItem curve = myPane.AddCurve(myLabel, list,Color.Black, SymbolType.Circle);
            curve.Line.IsVisible = false;
            curve.Symbol.Border.IsVisible = false;
            curve.Symbol.Fill = new Fill(Color.Black);
            
            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, Array myArray, Array myArray2, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();

            int index = 0;
            foreach (double bio in myArray)
            {
                list.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray2)
            {
                list2.Add(index, bio);
                index++;
            }
            string myLabel = (repCount-1).ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            LineItem curve = myPane.AddCurve(myLabel+"(1)", list, c, SymbolType.None);
            Color c2 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c2 = Color.FromArgb(2, 0, 0, 255);
            else
                c2 = c;
            LineItem curve2 = myPane.AddCurve(myLabel + "(2)", list2, c2, SymbolType.None);
            curve.Line.Width = 2;
            curve2.Line.Width = (float)1.0;
            if (menuBatchMode.Checked)
            {
                curve.Line.Width = 1;
                curve2.Line.Width = (float)0.5;
            }
            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
                curve2.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
                curve2.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, Array myArray, Array myArray2,Array myArray3, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();

            int index = 0;
            foreach (double bio in myArray)
            {
                list.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray2)
            {
                list2.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray3)
            {
                list3.Add(index, bio);
                index++;
            }
            string myLabel = (repCount-1).ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            LineItem curve = myPane.AddCurve(myLabel+"(1)", list, c, SymbolType.None);
            Color c2 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c2 = Color.FromArgb(2, 0, 0, 255);
            else
                c2 = c;
            LineItem curve2 = myPane.AddCurve(myLabel + "(2)", list2, c2, SymbolType.None);
            curve.Line.Width = 2;
            curve2.Line.Width = (float)1.5;
            Color c3 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c3 = Color.FromArgb(2, 255, 0, 0);
            else
                c3 = c;
            LineItem curve3 = myPane.AddCurve(myLabel + "(3)", list3, c3, SymbolType.None);
            curve.Line.Width = 2;
            curve3.Line.Width = (float)1;
            curve2.Line.Width = (float)1.0;
            if (menuBatchMode.Checked)
            {
                curve.Line.Width = 1;
                curve2.Line.Width = (float)0.5;
                curve3.Line.Width = (float)0.5;
            }
            else
            {
                curve3.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve3.Line.DashOn = 1;
                curve3.Line.DashOff = 30;
            }
            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
                curve2.Label.IsVisible = true;
                curve3.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
                curve2.Label.IsVisible = false;
                curve3.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, Array myArray, Array myArray2, Array myArray3, Array myArray4, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();

            int index = 0;
            foreach (double bio in myArray)
            {
                list.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray2)
            {
                list2.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray3)
            {
                list3.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray4)
            {
                list4.Add(index, bio);
                index++;
            }
            string myLabel = (repCount-1).ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            LineItem curve = myPane.AddCurve(myLabel + "(1)", list, c, SymbolType.None);
            Color c2 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c2 = Color.FromArgb(2, 0, 0, 255);
            else
                c2 = c;
            LineItem curve2 = myPane.AddCurve(myLabel + "(2)", list2, c2, SymbolType.None);
            curve.Line.Width = 2;
            curve2.Line.Width = (float)1.5;
            Color c3 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c3 = Color.FromArgb(2, 255, 0, 0);
            else
                c3 = c;
            LineItem curve3 = myPane.AddCurve(myLabel + "(3)", list3, c3, SymbolType.None);
            Color c4 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c4 = Color.FromArgb(2, 255, 0, 255);
            else
                c4 = c;
            LineItem curve4 = myPane.AddCurve(myLabel + "(4)", list4, c4, SymbolType.None);
            curve.Line.Width = 2;
            curve3.Line.Width = (float)1;
            curve2.Line.Width = (float)1.0;
            curve4.Line.Width = 2;
            if (menuBatchMode.Checked)
            {
                curve.Line.Width = 1;
                curve2.Line.Width = (float)0.5;
                curve3.Line.Width = (float)0.5;
                curve4.Line.Width = (float)0.5;
            }
            else
            {
                curve3.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve3.Line.DashOn = 1;
                curve3.Line.DashOff = 30;
                curve4.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve4.Line.DashOn = 1;
                curve4.Line.DashOff = 30;

            }
            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
                curve2.Label.IsVisible = true;
                curve3.Label.IsVisible = true;
                curve4.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
                curve2.Label.IsVisible = false;
                curve3.Label.IsVisible = false;
                curve4.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, Array myArray, Array myArray2, Array myArray3, Array myArray4, Array myArray5, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();
            PointPairList list5 = new PointPairList();

            int index = 0;
            foreach (double bio in myArray)
            {
                list.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray2)
            {
                list2.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray3)
            {
                list3.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray4)
            {
                list4.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray5)
            {
                list5.Add(index, bio);
                index++;
            }
            string myLabel = (repCount-1).ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            LineItem curve = myPane.AddCurve(myLabel + "(1)", list, c, SymbolType.None);
            Color c2 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c2 = Color.FromArgb(2, 0, 0, 255);
            else
                c2 = c;
            LineItem curve2 = myPane.AddCurve(myLabel + "(2)", list2, c2, SymbolType.None);
            curve.Line.Width = 2;
            curve2.Line.Width = (float)1.5;
            Color c3 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c3 = Color.FromArgb(2, 255, 0, 0);
            else
                c3 = c;
            LineItem curve3 = myPane.AddCurve(myLabel + "(3)", list3, c3, SymbolType.None);
            Color c4 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c4 = Color.FromArgb(2, 255, 0, 255);
            else
                c4 = c;
            LineItem curve4 = myPane.AddCurve(myLabel + "(4)", list4, c4, SymbolType.None);
            Color c5 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c5 = Color.FromArgb(2, 0, 255, 0);
            else
                c5 = c;
            LineItem curve5 = myPane.AddCurve(myLabel + "(5)", list5, c5, SymbolType.None);
           
            curve.Line.Width = 2;
            curve3.Line.Width = (float)1;
            curve2.Line.Width = (float)1.0;
            curve4.Line.Width = 2;
            curve5.Line.Width = (float)1;
            if (menuBatchMode.Checked)
            {
                curve.Line.Width = 1;
                curve2.Line.Width = (float)0.5;
                curve3.Line.Width = (float)0.5;
                curve4.Line.Width = (float)0.5;
                curve5.Line.Width = (float)0.5;
            }
            else
            {
                curve3.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve3.Line.DashOn = 1;
                curve3.Line.DashOff = 30;
                curve4.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve4.Line.DashOn = 1;
                curve4.Line.DashOff = 30;
                curve5.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve5.Line.DashOn = 10;
                curve5.Line.DashOff = 90;

            }
            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
                curve2.Label.IsVisible = true;
                curve3.Label.IsVisible = true;
                curve4.Label.IsVisible = true;
                curve5.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
                curve2.Label.IsVisible = false;
                curve3.Label.IsVisible = false;
                curve4.Label.IsVisible = false;
                curve5.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, Array myArray, Array myArray2, Array myArray3, Array myArray4, Array myArray5, Array myArray6, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;

            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();
            PointPairList list5 = new PointPairList();
            PointPairList list6 = new PointPairList();

            int index = 0;
            foreach (double bio in myArray)
            {
                list.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray2)
            {
                list2.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray3)
            {
                list3.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray4)
            {
                list4.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray5)
            {
                list5.Add(index, bio);
                index++;
            }
            index = 0;
            foreach (double bio in myArray6)
            {
                list6.Add(index, bio);
                index++;
            }
            string myLabel = (repCount-1).ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            LineItem curve = myPane.AddCurve(myLabel + "(1)", list, c, SymbolType.None);
            Color c2 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c2 = Color.FromArgb(2, 0, 0, 255);
            else
                c2 = c;
            LineItem curve2 = myPane.AddCurve(myLabel + "(2)", list2, c2, SymbolType.None);
            curve.Line.Width = 2;
            curve2.Line.Width = (float)1.5;
            Color c3 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c3 = Color.FromArgb(2, 255, 0, 0);
            else
                c3 = c;
            LineItem curve3 = myPane.AddCurve(myLabel + "(3)", list3, c3, SymbolType.None);
            Color c4 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c4 = Color.FromArgb(2, 255, 0, 255);
            else
                c4 = c;
            LineItem curve4 = myPane.AddCurve(myLabel + "(4)", list4, c4, SymbolType.None);
            Color c5 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c5 = Color.FromArgb(2, 0, 255, 0);
            else
                c5 = c;
            LineItem curve5 = myPane.AddCurve(myLabel + "(5)", list5, c5, SymbolType.None);
            Color c6 = new Color();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c6 = Color.FromArgb(2, 0, 255, 255);
            else
                c6 = c;
            LineItem curve6 = myPane.AddCurve(myLabel + "(6)", list6, c6, SymbolType.None);

            curve.Line.Width = 2;
            curve3.Line.Width = (float)1;
            curve2.Line.Width = (float)1.0;
            curve4.Line.Width = 2;
            curve5.Line.Width = (float)1;
            curve6.Line.Width = (float)2;
            if (menuBatchMode.Checked)
            {
                curve.Line.Width = 1;
                curve2.Line.Width = (float)0.5;
                curve3.Line.Width = (float)0.5;
                curve4.Line.Width = (float)0.5;
                curve5.Line.Width = (float)0.5;
                curve6.Line.Width = (float)0.5;
            }
            else
            {
                curve3.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve3.Line.DashOn = 1;
                curve3.Line.DashOff = 30;
                curve4.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve4.Line.DashOn = 1;
                curve4.Line.DashOff = 30;
                curve5.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve5.Line.DashOn = 10;
                curve5.Line.DashOff = 90;
                curve6.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                curve6.Line.DashOn = 10;
                curve6.Line.DashOff = 90;

            }
            if (checkBoxLegend.Checked)
            {
                curve.Label.IsVisible = true;
                curve2.Label.IsVisible = true;
                curve3.Label.IsVisible = true;
                curve4.Label.IsVisible = true;
                curve5.Label.IsVisible = true;
                curve6.Label.IsVisible = true;
            }
            else
            {
                curve.Label.IsVisible = false;
                curve2.Label.IsVisible = false;
                curve3.Label.IsVisible = false;
                curve4.Label.IsVisible = false;
                curve5.Label.IsVisible = false;
                curve6.Label.IsVisible = false;
            }
            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, List<List<double []>> coList, double [] LAITotalArray, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;
            int sppCount = 0;
            if (!menuBatchMode.Checked)
            {
                foreach (List<double[]> tempCoList in coList)
                {
                    sppCount += 1;



                    foreach (double[] myArray in tempCoList)
                    {
                        int index = 0;
                        PointPairList list = new PointPairList();
                        foreach (double bio in myArray)
                        {
                            list.Add(index, bio);
                            index++;
                        }
                        string myLabel = (repCount-1).ToString();
                        if (menuBatchMode.Checked && !cbRange.Checked)
                        {
                            if(sppCount == 1)
                                c = Color.FromArgb(2, 0, 0, 0);
                            if (sppCount == 2)
                                c = Color.FromArgb(2, 0, 0, 255);
                            if (sppCount == 3)
                                c = Color.FromArgb(2, 255, 0, 0);
                            if (sppCount == 4)
                                c = Color.FromArgb(2, 255, 0, 255);
                            if (sppCount == 5)
                                c = Color.FromArgb(2, 0, 255,0);
                            if (sppCount == 6)
                                c = Color.FromArgb(2, 0, 255,255);
                        }
                      
                        LineItem curve = myPane.AddCurve(myLabel + "(" + sppCount.ToString() + ")", list, c, SymbolType.None);
                        curve.Line.Width = (float)0.5;
                        if (!menuBatchMode.Checked)
                        {
                            if (sppCount == 1)
                            {
                                curve.Line.Width = 2;
                            }
                            if (sppCount == 2)
                            {
                                curve.Line.Width = 1;
                            }
                            if (sppCount == 3)
                            {
                                curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                                curve.Line.DashOn = 1;
                                curve.Line.DashOff = 30;
                            }
                            if (sppCount == 4)
                            {
                                curve.Line.Width = 2;
                                curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                                curve.Line.DashOn = 1;
                                curve.Line.DashOff = 30;
                            }
                            if (sppCount == 5)
                            {
                                curve.Line.Width = 1;
                                curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                                curve.Line.DashOn = 10;
                                curve.Line.DashOff = 90;
                            }
                            if (sppCount == 6)
                            {
                                curve.Line.Width = 2;
                                curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                                curve.Line.DashOn = 10;
                                curve.Line.DashOff = 90;
                            }
                           
                        }

                        if (checkBoxLegend.Checked)
                        {
                            curve.Label.IsVisible = true;
                        }
                        else
                        {
                            curve.Label.IsVisible = false;
                        }

                    }
                }
            }
            PointPairList list2 = new PointPairList();

            int index2 = 0;
            foreach (double bio in LAITotalArray)
            {
                list2.Add(index2, bio);
                index2++;
            }
            string myLabel2 = repCount.ToString();
            if (menuBatchMode.Checked && !cbRange.Checked)
                c = Color.FromArgb(2, 0, 0, 0);
            else if(!cbRange.Checked)
                c = Color.FromArgb(0, 0, 0);
            LineItem curve2 = myPane.AddCurve(myLabel2, list2, c, SymbolType.None);
            if (menuBatchMode.Checked)
            {
                curve2.Line.Width = (float) 0.5;
            }
            else{
            curve2.Line.Width = 2;
            }
            if (checkBoxLegend.Checked)
            {
                curve2.Label.IsVisible = true;
            }
            else
            {
                curve2.Label.IsVisible = false;
            }

            zgc.AxisChange();

            return myPane;

        }
        private GraphPane CreateGraph(ZedGraphControl zgc, string yLabel, List<List<double[]>> coList, Color c)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = yLabel;
            myPane.XAxis.Title.Text = "Year";
            myPane.YAxis.Title.Text = yLabel;
            int sppCount = 0;
            foreach (List<double[]> tempCoList in coList)
            {
                sppCount += 1;



                foreach (double[] myArray in tempCoList)
                {
                    int index = 0;
                    PointPairList list = new PointPairList();
                    foreach (double bio in myArray)
                    {
                        list.Add(index, bio);
                        index++;
                    }
                    string myLabel = (repCount-1).ToString();
                    if (menuBatchMode.Checked && !cbRange.Checked)
                    {
                        if (sppCount == 1)
                            c = Color.FromArgb(2, 0, 0, 0);
                        if (sppCount == 2)
                            c = Color.FromArgb(2, 0, 0, 255);
                        if (sppCount == 3)
                            c = Color.FromArgb(2, 255, 0, 0);
                        if (sppCount == 4)
                            c = Color.FromArgb(2, 255, 0, 255);
                        if (sppCount == 5)
                            c = Color.FromArgb(2, 0, 255,0);
                        if (sppCount == 6)
                            c = Color.FromArgb(2, 0, 255,255);
                    }
                    LineItem curve = myPane.AddCurve(myLabel + "(" + sppCount.ToString() + ")", list, c, SymbolType.None);
                    curve.Line.Width = (float)0.5;
                    if (!menuBatchMode.Checked)
                    {
                        if (sppCount == 1)
                        {
                            curve.Line.Width = 2;
                        }
                        if(sppCount == 2)
                        {
                            curve.Line.Width = 1;
                        }
                        if (sppCount == 3)
                        {
                            curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                            curve.Line.DashOn = 1;
                            curve.Line.DashOff = 30;
                        }
                        if (sppCount == 4)
                        {
                            curve.Line.Width = 2;
                            curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                            curve.Line.DashOn = 1;
                            curve.Line.DashOff = 30;
                        }
                        if (sppCount == 5)
                        {
                            curve.Line.Width = 1;
                            curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                            curve.Line.DashOn = 10;
                            curve.Line.DashOff = 90;
                        }
                        if (sppCount == 6)
                        {
                            curve.Line.Width = 2;
                            curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
                            curve.Line.DashOn = 10;
                            curve.Line.DashOff = 90;
                        }

                    }
                    if (checkBoxLegend.Checked)
                    {
                        curve.Label.IsVisible = true;
                    }
                    else
                    {
                        curve.Label.IsVisible = false;
                    }
                }
            }
                       
            zgc.AxisChange();

            return myPane;

        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            GraphPane myPane1 = graph1.GraphPane;
            myPane1.CurveList.Clear();
            myPane1.GraphObjList.Clear();
            GraphPane myPane2 = graph2.GraphPane;
            myPane2.CurveList.Clear();
            myPane2.GraphObjList.Clear();
            GraphPane myPane3 = graph3.GraphPane;
            myPane3.CurveList.Clear();
            myPane3.GraphObjList.Clear();
            GraphPane myPane4 = graph4.GraphPane;
            myPane4.CurveList.Clear();
            myPane4.GraphObjList.Clear();
            GraphPane myPane6 = graph6.GraphPane;
            myPane6.CurveList.Clear();
            myPane6.GraphObjList.Clear();
            GraphPane myPane9 = graph9.GraphPane;
            myPane9.CurveList.Clear();
            myPane9.GraphObjList.Clear();
            GraphPane myPane10 = graph10.GraphPane;
            myPane10.CurveList.Clear();
            myPane10.GraphObjList.Clear();
            //Budworm - Clear budworm graphs
            GraphPane myPane5 = graph_BudwormD.GraphPane;
            myPane5.CurveList.Clear();
            myPane5.GraphObjList.Clear();
            graph_BudwormD.Refresh();
            GraphPane myPane7 = graph_BudwormC.GraphPane;
            myPane7.CurveList.Clear();
            myPane7.GraphObjList.Clear();
            graph_BudwormC.Refresh();
            GraphPane myPane8 = graph_EnemyDens.GraphPane;
            myPane8.CurveList.Clear();
            myPane8.GraphObjList.Clear();
            graph_EnemyDens.Refresh();
            GraphPane myPane11 = graph_EnemyCount.GraphPane;
            myPane11.CurveList.Clear();
            myPane11.GraphObjList.Clear();
            graph_EnemyCount.Refresh();
            GraphPane myPane12 = graph_HostFoliage.GraphPane;
            myPane12.CurveList.Clear();
            myPane12.GraphObjList.Clear();
            graph_HostFoliage.Refresh();
            GraphPane myPane13 = graph_PropDefol.GraphPane;
            myPane13.CurveList.Clear();
            myPane13.GraphObjList.Clear();
            graph_PropDefol.Refresh();
            GraphPane myPane14 = graph_LogPopDens.GraphPane;
            myPane14.CurveList.Clear();
            myPane14.GraphObjList.Clear();
            graph_LogPopDens.Refresh();
            GraphPane myPane15 = graph_SBWmortality.GraphPane;
            myPane15.CurveList.Clear();
            myPane15.GraphObjList.Clear();
            graph_SBWmortality.Refresh();
            GraphPane myPane16 = graph_SBWmigration.GraphPane;
            myPane16.CurveList.Clear();
            myPane16.GraphObjList.Clear();
            graph_SBWmigration.Refresh();
            //Budworm - END - clear budworm graphs

            graph1.Refresh();
            graph2.Refresh();
            graph3.Refresh();
            graph4.Refresh();
            graph6.Refresh();
            graph9.Refresh();
            graph10.Refresh();
            
            repCount = 1;

        }

        private void rbVer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbV30.Checked)
            {
                label9.Enabled = true;
                tbLeaf.Enabled = true;
                tbLeaf2.Enabled = true;
                tbLeaf3.Enabled = true;
                tbLeaf4.Enabled = true;
                tbLeaf5.Enabled = true;
                tbLeaf6.Enabled = true;

                label24.Enabled = true;
                tbDecay1.Enabled = true;
                tbDecay2.Enabled = true;
                tbDecay3.Enabled = true;
                tbDecay4.Enabled = true;
                tbDecay5.Enabled = true;
                tbDecay6.Enabled = true;

                groupBox8.Enabled = true;
                label4.Enabled = true;
                tbANPPmax.Enabled = true;
                tbANPPmax2.Enabled = true;
                tbANPPmax3.Enabled = true;
                tbANPPmax4.Enabled = true;
                tbANPPmax5.Enabled = true;
                tbANPPmax6.Enabled = true;

                label5.Enabled = true;
                tbBioMax.Enabled = true;
                tbBioMax2.Enabled = true;
                tbBioMax3.Enabled = true;
                tbBioMax4.Enabled = true;
                tbBioMax5.Enabled = true;
                tbBioMax6.Enabled = true;

                label7.Enabled = true;
                tbMortShape1.Enabled = true;
                tbMortShape2.Enabled = true;
                tbMortShape3.Enabled = true;
                tbMortShape4.Enabled = true;
                tbMortShape5.Enabled = true;
                tbMortShape6.Enabled = true;

                label26.Enabled = true;
                tbPower1.Enabled = true;
                tbPower2.Enabled = true;
                tbPower3.Enabled = true;
                tbPower4.Enabled = true;
                tbPower5.Enabled = true;
                tbPower6.Enabled = true;

                groupBox4.Enabled = true;

                label22.Enabled = true;
                tbRelBio1.Enabled = true;
                tbRelBio2.Enabled = true;
                tbRelBio3.Enabled = true;
                tbRelBio4.Enabled = true;
                tbRelBio5.Enabled = true;

                cbRangeMortShape1.Enabled = true;
                cbRangeMortShape2.Enabled = true;
                cbRangeMortShape3.Enabled = true;
                cbRangeMortShape4.Enabled = true;
                cbRangeMortShape5.Enabled = true;
                cbRangeMortShape6.Enabled = true;

                label29.Enabled = false;
                tbMortMod1.Enabled = false;
                tbMortMod2.Enabled = false;
                tbMortMod3.Enabled = false;
                tbMortMod4.Enabled = false;
                tbMortMod5.Enabled = false;
                tbMortMod6.Enabled = false;

                label6.Enabled = false;
                tbAgeMort1.Enabled = false;
                tbAgeMort2.Enabled = false;
                tbAgeMort3.Enabled = false;
                tbAgeMort4.Enabled = false;
                tbAgeMort5.Enabled = false;
                tbAgeMort6.Enabled = false;
                
                if (menuBatchMode.Checked)
                {
                    cbRange.Enabled = true;
                }

                cbRangeAP1.Enabled = true;
                cbRangeAP2.Enabled = true;
                cbRangeAP3.Enabled = true;
                cbRangeAP4.Enabled = true;
                cbRangeAP5.Enabled = true;
                cbRangeAP6.Enabled = true;
                cbRangeMM1.Enabled = false;
                cbRangeMM2.Enabled = false;
                cbRangeMM3.Enabled = false;
                cbRangeMM4.Enabled = false;
                cbRangeMM5.Enabled = false;
                cbRangeMM6.Enabled = false;
                cbRangeAgeMort1.Enabled = false;
                cbRangeAgeMort2.Enabled = false;
                cbRangeAgeMort3.Enabled = false;
                cbRangeAgeMort4.Enabled = false;
                cbRangeAgeMort5.Enabled = false;
                cbRangeAgeMort5.Enabled = false;

                groupBox7.Enabled = true;

                label10.Enabled = false;
                tbMatAge1.Enabled = false;
                tbMatAge2.Enabled = false;
                tbMatAge3.Enabled = false;
                tbMatAge4.Enabled = false;
                tbMatAge5.Enabled = false;
                tbMatAge5.Enabled = false;

            }
            else if (rbV35.Checked)
            {
                label9.Enabled = true;
                tbLeaf.Enabled = true;
                tbLeaf2.Enabled = true;
                tbLeaf3.Enabled = true;
                tbLeaf4.Enabled = true;
                tbLeaf5.Enabled = true;
                tbLeaf6.Enabled = true;

                label24.Enabled = true;
                tbDecay1.Enabled = true;
                tbDecay2.Enabled = true;
                tbDecay3.Enabled = true;
                tbDecay4.Enabled = true;
                tbDecay5.Enabled = true;
                tbDecay6.Enabled = true;

                groupBox8.Enabled = true;
                label4.Enabled = true;
                tbANPPmax.Enabled = true;
                tbANPPmax2.Enabled = true;
                tbANPPmax3.Enabled = true;
                tbANPPmax4.Enabled = true;
                tbANPPmax5.Enabled = true;
                tbANPPmax6.Enabled = true;

                label5.Enabled = true;
                tbBioMax.Enabled = true;
                tbBioMax2.Enabled = true;
                tbBioMax3.Enabled = true;
                tbBioMax4.Enabled = true;
                tbBioMax5.Enabled = true;
                tbBioMax6.Enabled = true;

                label7.Enabled = false;
                tbMortShape1.Enabled = false;
                tbMortShape2.Enabled = false;
                tbMortShape3.Enabled = false;
                tbMortShape4.Enabled = false;
                tbMortShape5.Enabled = false;
                tbMortShape6.Enabled = false;

                cbRangeMortShape1.Enabled = false;
                cbRangeMortShape2.Enabled = false;
                cbRangeMortShape3.Enabled = false;
                cbRangeMortShape4.Enabled = false;
                cbRangeMortShape5.Enabled = false;
                cbRangeMortShape6.Enabled = false;

                groupBox4.Enabled = true;

                label22.Enabled = true;
                tbRelBio1.Enabled = true;
                tbRelBio2.Enabled = true;
                tbRelBio3.Enabled = true;
                tbRelBio4.Enabled = true;
                tbRelBio5.Enabled = true;

                label26.Enabled = true;
                tbPower1.Enabled = true;
                tbPower2.Enabled = true;
                tbPower3.Enabled = true;
                tbPower4.Enabled = true;
                tbPower5.Enabled = true;
                tbPower6.Enabled = true;

                label29.Enabled = true;
                tbMortMod1.Enabled = true;
                tbMortMod2.Enabled = true;
                tbMortMod3.Enabled = true;
                tbMortMod4.Enabled = true;
                tbMortMod5.Enabled = true;
                tbMortMod6.Enabled = true;

                label6.Enabled = true;
                tbAgeMort1.Enabled = true;
                tbAgeMort2.Enabled = true;
                tbAgeMort3.Enabled = true;
                tbAgeMort4.Enabled = true;
                tbAgeMort5.Enabled = true;
                tbAgeMort6.Enabled = true;

                if (menuBatchMode.Checked)
                {
                    cbRange.Enabled = true;
                }

                cbRangeAP1.Enabled = true;
                cbRangeAP2.Enabled = true;
                cbRangeAP3.Enabled = true;
                cbRangeAP4.Enabled = true;
                cbRangeAP5.Enabled = true;
                cbRangeAP6.Enabled = true;
                cbRangeMM1.Enabled = true;
                cbRangeMM2.Enabled = true;
                cbRangeMM3.Enabled = true;
                cbRangeMM4.Enabled = true;
                cbRangeMM5.Enabled = true;
                cbRangeMM6.Enabled = true;
                cbRangeAgeMort1.Enabled = true;
                cbRangeAgeMort2.Enabled = true;
                cbRangeAgeMort3.Enabled = true;
                cbRangeAgeMort4.Enabled = true;
                cbRangeAgeMort5.Enabled = true;
                cbRangeAgeMort6.Enabled = true;

                groupBox7.Enabled = true;
                
                label10.Enabled = false;
                tbMatAge1.Enabled = false;
                tbMatAge2.Enabled = false;
                tbMatAge3.Enabled = false;
                tbMatAge4.Enabled = false;
                tbMatAge5.Enabled = false;
                tbMatAge6.Enabled = false;

            }
            else if (rbAgeList.Checked)
            {
                label9.Enabled = false;
                tbLeaf.Enabled = false;
                tbLeaf2.Enabled = false;
                tbLeaf3.Enabled = false;
                tbLeaf4.Enabled = false;
                tbLeaf5.Enabled = false;
                tbLeaf6.Enabled = false;

                label24.Enabled = false;
                tbDecay1.Enabled = false;
                tbDecay2.Enabled = false;
                tbDecay3.Enabled = false;
                tbDecay4.Enabled = false;
                tbDecay5.Enabled = false;
                tbDecay6.Enabled = false;

                groupBox8.Enabled = false;
                
                groupBox4.Enabled = false;

                cbRange.Enabled = false;

                groupBox7.Enabled = false;

                label10.Enabled = true;
                tbMatAge1.Enabled = true;
                tbMatAge2.Enabled = true;
                tbMatAge3.Enabled = true;
                tbMatAge4.Enabled = true;
                tbMatAge5.Enabled = true;
                tbMatAge6.Enabled = true;

            }
            
        }






        private void tbSimYears_Leave(object sender, EventArgs e)
        {
            int test;
            try
            {
                test = int.Parse(tbSimYears.Text);
                if (test <= 0)
                {
                    string mesg = "Simulation years must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbSimYears.Focus();
                    tbSimYears.SelectAll();
                }

            }
            catch
            {
                string mesg = "Simulation years must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbSimYears.Focus();
                tbSimYears.SelectAll();
            }



        }

        

        private void tbLongevity_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if (test <= 0)
                {
                    string mesg = "Longevity must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Longevity must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbShadeTol_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test <= 0) | (test > 5))
                {
                    string mesg = "Shade tolerance must be an integer from 1 to 5.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Shade tolerance must be an integer from 1 to 5.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbMaturity_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "Age of maturity must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Age of maturity must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbANPPMax_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "ANPP maximum must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "ANPP maximum must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbBiomassMax_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "Biomass maximum must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Biomass maximum must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbLeaf_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "Leaf longevity must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Leaf longevity must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbPctBioMaxLAI_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test < 0) | (test > 100))
                {
                    string mesg = "Percent biomass at maximum LAI must be an integer between 0 and 100.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Percent biomass at maximum LAI must be an integer between 0 and 100.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbClass_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test < 0) | (test > 100))
                {
                    string mesg = "Shade threshold must be an integer between 0 and 100.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Shade threshold must be an integer between 0 and 100.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbD_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            double test;
            try
            {
                test = double.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "Mortality shape parameter must be a decimal greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Mortality shape parameter must be a decimal greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbMaxLAI_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            double test;
            try
            {
                test = double.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "Maximum LAI must be a number greater than 0.0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Maximum LAI must be a number greater than 0.0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbK_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            double test;
            try
            {
                test = double.Parse(testBox.Text);
                if ((test <= 0) | (test > 1))
                {
                    string mesg = "K must be a decimal between 0 and 1.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "K must be a decimal between 0 and 1.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }
        private void tbEstab_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            double test;
            try
            {
                test = double.Parse(testBox.Text);
                if ((test < 0) | (test > 1))
                {
                    string mesg = "Establishment probability must be a decimal from 0 to 1.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Establishment probability must be a decimal from 0 to 1.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }

        private void menuGraphLegend_Click(object sender, EventArgs e)
        {
            if (menuGraphLegend.Checked)
            {
                menuGraphLegend.Checked = false;
                checkBoxLegend.Checked = false;
            }
            else
            {
                menuGraphLegend.Checked = true;
                checkBoxLegend.Checked = true;
            }
        }

        private void menuBatchMode_Click(object sender, EventArgs e)
        {
            if (menuBatchMode.Checked)  //Change checked to unchecked
            {
                menuBatchMode.Checked = false;
                label11.Enabled = false;
                tbBatch.Enabled = false;
                checkBoxBatch.Checked = false;
                cbRange.Enabled = false;
                cbRangeAgeMort1.Visible = false;
                cbRangeAgeMort2.Visible = false;
                cbRangeAgeMort3.Visible = false;
                cbRangeAgeMort4.Visible = false;
                cbRangeMM1.Visible = false;
                cbRangeMM2.Visible = false;
                cbRangeMM3.Visible = false;
                cbRangeMM4.Visible = false;
                cbRangeAP1.Visible = false;
                cbRangeAP2.Visible = false;
                cbRangeAP3.Visible = false;
                cbRangeAP4.Visible = false;
                cbANPP1.Visible = false;
                cbANPP2.Visible = false;
                cbANPP3.Visible = false;
                cbANPP4.Visible = false;
                cbRangeMortShape1.Visible = false;
                cbRangeMortShape2.Visible = false;
                cbRangeMortShape3.Visible = false;
                cbRangeMortShape4.Visible = false;
                cbRandSeed.Enabled = true;
                if (cbRandSeed.Checked)
                {
                    label99.Enabled = true;
                    tbRandSeed.Enabled = true;
                }
                else
                {
                    label99.Enabled = false;
                    tbRandSeed.Enabled = false;
                }

            }
            else  // Change unchecked to checked
            {
                menuBatchMode.Checked = true;
                label11.Enabled = true;
                tbBatch.Enabled = true;
                checkBoxBatch.Checked = true;
                if (rbV30.Checked || rbV35.Checked)
                {
                    cbRange.Enabled = true;
                    if (cbRange.Checked)
                    {
                        cbRangeAgeMort1.Visible = true;
                        cbRangeAgeMort2.Visible = true;
                        cbRangeAgeMort3.Visible = true;
                        cbRangeAgeMort4.Visible = true;
                        cbRangeMM1.Visible = true;
                        cbRangeMM2.Visible = true;
                        cbRangeMM3.Visible = true;
                        cbRangeMM4.Visible = true;
                        cbRangeAP1.Visible = true;
                        cbRangeAP2.Visible = true;
                        cbRangeAP3.Visible = true;
                        cbRangeAP4.Visible = true;
                        cbANPP1.Visible = true;
                        cbANPP2.Visible = true;
                        cbANPP3.Visible = true;
                        cbANPP4.Visible = true;
                        cbRangeMortShape1.Visible = true;
                        cbRangeMortShape2.Visible = true;
                        cbRangeMortShape3.Visible = true;
                        cbRangeMortShape4.Visible = true;
                    }
                    else
                    {
                        cbRangeAgeMort1.Visible = false;
                        cbRangeAgeMort2.Visible = false;
                        cbRangeAgeMort3.Visible = false;
                        cbRangeAgeMort4.Visible = false;
                        cbRangeMM1.Visible = false;
                        cbRangeMM2.Visible = false;
                        cbRangeMM3.Visible = false;
                        cbRangeMM4.Visible = false;
                        cbRangeAP1.Visible = false;
                        cbRangeAP2.Visible = false;
                        cbRangeAP3.Visible = false;
                        cbRangeAP4.Visible = false;
                        cbANPP1.Visible = false;
                        cbANPP2.Visible = false;
                        cbANPP3.Visible = false;
                        cbANPP4.Visible = false;
                        cbRangeMortShape1.Visible = false;
                        cbRangeMortShape2.Visible = false;
                        cbRangeMortShape3.Visible = false;
                        cbRangeMortShape4.Visible = false;
                    }
                    cbRandSeed.Enabled = false;
                    label99.Enabled = false;
                    tbRandSeed.Enabled = false;
                }

            }

        }

        private void tbSppNum_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test < 1) | (test > 6))
                {
                    string mesg = "Species number must be an integer between 1 and 6.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Species number must be an integer between 1 and 6.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }

        private void menuOutputLog_Click(object sender, EventArgs e)
        {
            if (menuOutputLog.Checked)
            {
                menuOutputLog.Checked = false;
                checkBoxLog.Checked = false;
                cbOutputFolder.Enabled = false;
                tbBaseFileName.Enabled = false;
            }
            else
            {
                menuOutputLog.Checked = true;
                checkBoxLog.Checked = true;
                cbOutputFolder.Enabled = true;
                tbBaseFileName.Enabled = true;
            }
        }




        private bool CheckSppNum(int sppNum1, int sppNum2, int sppNum3, int sppNum4, int sppNum5, int sppNum6)
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            int count5 = 0;
            int count6 = 0;

            if (sppNum1 == 1)
                count1 += 1;
            else if (sppNum1 == 2)
                count2 += 1;
            else if (sppNum1 == 3)
                count3 += 1;
            else if (sppNum1 == 4)
                count4 += 1;
            else if (sppNum1 == 5)
                count5 += 1;
            else if (sppNum1 == 6)
                count6 += 1;
            if (sppNum2 == 1)
                count1 += 1;
            else if (sppNum2 == 2)
                count2 += 1;
            else if (sppNum2 == 3)
                count3 += 1;
            else if (sppNum2 == 4)
                count4 += 1;
            else if (sppNum2 == 5)
                count5 += 1;
            else if (sppNum2 == 6)
                count6 += 1;
            if (sppNum3 == 1)
                count1 += 1;
            else if (sppNum3 == 2)
                count2 += 1;
            else if (sppNum3 == 3)
                count3 += 1;
            else if (sppNum3 == 4)
                count4 += 1;
            else if (sppNum3 == 5)
                count5 += 1;
            else if (sppNum3 == 6)
                count6 += 1;
            if (sppNum4 == 1)
                count1 += 1;
            else if (sppNum4 == 2)
                count2 += 1;
            else if (sppNum4 == 3)
                count3 += 1;
            else if (sppNum4 == 4)
                count4 += 1;
            else if (sppNum4 == 5)
                count5 += 1;
            else if (sppNum4 == 6)
                count6 += 1;
            if (sppNum5 == 1)
                count1 += 1;
            else if (sppNum5 == 2)
                count2 += 1;
            else if (sppNum5 == 3)
                count3 += 1;
            else if (sppNum5 == 4)
                count4 += 1;
            else if (sppNum5 == 5)
                count5 += 1;
            else if (sppNum5 == 6)
                count6 += 1;
            if (sppNum6 == 1)
                count1 += 1;
            else if (sppNum6 == 2)
                count2 += 1;
            else if (sppNum6 == 3)
                count3 += 1;
            else if (sppNum6 == 4)
                count4 += 1;
            else if (sppNum6 == 5)
                count5 += 1;
            else if (sppNum6 == 6)
                count6 += 1;
            if ((count1 != 1) | (count2 != 1) | (count3 != 1) | (count4 != 1) | (count5 != 1) | (count6 != 1))
                return false;
            else
                return true;


        }

        private void tbReps_Leave(object sender, EventArgs e)
        {
            TextBox testBox = (TextBox)sender;
            int test;
            try
            {
                test = int.Parse(testBox.Text);
                if ((test <= 0))
                {
                    string mesg = "Number of replicates must be an integer greater than 0.";
                    MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    testBox.Focus();
                    testBox.SelectAll();
                }
            }
            catch
            {
                string mesg = "Number of replicates must be an integer greater than 0.";
                MessageBox.Show(mesg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                testBox.Focus();
                testBox.SelectAll();
            }
        }


 


        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void cbRange_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRange.Checked)
            {
                cbRangeAgeMort1.Visible = true;
                cbRangeAgeMort2.Visible = true;
                cbRangeAgeMort3.Visible = true;
                cbRangeAgeMort4.Visible = true;
                cbRangeAgeMort5.Visible = true;
                cbRangeAgeMort6.Visible = true;
                cbRangeMM1.Visible = true;
                cbRangeMM2.Visible = true;
                cbRangeMM3.Visible = true;
                cbRangeMM4.Visible = true;
                cbRangeMM5.Visible = true;
                cbRangeMM6.Visible = true;
                cbRangeAP1.Visible = true;
                cbRangeAP2.Visible = true;
                cbRangeAP3.Visible = true;
                cbRangeAP4.Visible = true;
                cbRangeAP5.Visible = true;
                cbRangeAP6.Visible = true;
                cbANPP1.Visible = true;
                cbANPP2.Visible = true;
                cbANPP3.Visible = true;
                cbANPP4.Visible = true;
                cbANPP5.Visible = true;
                cbANPP6.Visible = true;
                cbRangeMortShape1.Visible = true;
                cbRangeMortShape2.Visible = true;
                cbRangeMortShape3.Visible = true;
                cbRangeMortShape4.Visible = true;
                cbRangeMortShape5.Visible = true;
                cbRangeMortShape6.Visible = true;
            }
            else
            {
                cbRangeAgeMort1.Visible = false;
                cbRangeAgeMort2.Visible = false;
                cbRangeAgeMort3.Visible = false;
                cbRangeAgeMort4.Visible = false;
                cbRangeAgeMort5.Visible = false;
                cbRangeAgeMort6.Visible = false;
                cbRangeMM1.Visible = false;
                cbRangeMM2.Visible = false;
                cbRangeMM3.Visible = false;
                cbRangeMM4.Visible = false;
                cbRangeMM5.Visible = false;
                cbRangeMM6.Visible = false;
                cbRangeAP1.Visible = false;
                cbRangeAP2.Visible = false;
                cbRangeAP3.Visible = false;
                cbRangeAP4.Visible = false;
                cbRangeAP5.Visible = false;
                cbRangeAP6.Visible = false;
                cbANPP1.Visible = false;
                cbANPP2.Visible = false;
                cbANPP3.Visible = false;
                cbANPP4.Visible = false;
                cbANPP5.Visible = false;
                cbANPP6.Visible = false;
                cbRangeMortShape1.Visible = false;
                cbRangeMortShape2.Visible = false;
                cbRangeMortShape3.Visible = false;
                cbRangeMortShape4.Visible = false;
                cbRangeMortShape5.Visible = false;
                cbRangeMortShape6.Visible = false;
            }
        }

        private void addRefButton_Click(object sender, EventArgs e)
        {
            bool runRef = false;
            Form3 refForm = new Form3();
            refForm.ShowDialog();
            runRef = refForm.cbRef.Checked;
            
            Dictionary<int, double> refArray = new Dictionary<int, double>();

            if (runRef)
            {
                string fileName = refForm.tbFileName.Text;
                StreamReader fileReader;
                fileReader = new StreamReader(fileName);
                string line;
                
                do
                {
                    line = fileReader.ReadLine();

                    string[] inputList = Regex.Split(line, "\\s+");

                    if (inputList[0] != "Age")
                    {
                        int age = (int) Math.Round( double.Parse(inputList[0]));
                        double bio = double.Parse(inputList[1]);
                        refArray.Add(age, bio);
                    }
                } while (!fileReader.EndOfStream);
            }

            foreach (object item in refForm.listBox1.SelectedItems)
            {
                string graphName = item.ToString();
                if (graphName == "Biomass")
                {
                    GraphPane pane1 = CreateGraphPoints(graph1, "Biomass", refArray);
                    graph1.Refresh();
                }
                else if (graphName == "PctShade")
                {
                    GraphPane pane2 = CreateGraphPoints(graph2, "PctShade", refArray);
                    graph2.Refresh();
                }
                else if (graphName == "ShadeClass")
                {
                    GraphPane pane3 = CreateGraphPoints(graph3, "ShadeClass", refArray);
                    graph3.Refresh();
                }
                else if (graphName == "Num Cohorts")
                {
                    GraphPane pane4 = CreateGraphPoints(graph4, "Num Cohorts", refArray);
                    graph4.Refresh();
                }
                else if (graphName == "Cohort ANPP")
                {
                    GraphPane pane6 = CreateGraphPoints(graph6, "Cohort ANPP", refArray);
                    graph6.Refresh();
                }
                else if (graphName == "Cohort Biomass")
                {
                    GraphPane pane9 = CreateGraphPoints(graph9, "Cohort Biomass", refArray);
                    graph9.Refresh();
                }
                else if (graphName == "Dead Wood")
                {
                    GraphPane pane10 = CreateGraphPoints(graph10, "Dead Wood", refArray);
                    graph10.Refresh();
                }
                //Budworm
                // plot graphs
                else if (graphName == "Budworm Density")
                {
                    GraphPane pane5 = CreateGraphPoints(graph_BudwormD, "Budworm Density", refArray);
                    graph_BudwormD.Refresh();
                }
                else if (graphName == "Budworm Count")
                {
                    GraphPane pane7 = CreateGraphPoints(graph_BudwormC, "Budworm Count", refArray);
                    graph_BudwormC.Refresh();
                }
                //Budworm - END - plot graphs
            }

        }

        public class CohortComparer : IComparer<Cohort>
        {
            public int Compare(Cohort a, Cohort b)
            {
                double age1 = a.Age;
                double age2 = b.Age;
               
                if ((a.Species.Index == 0) && shrub1)
                {
                        if (age1 > (a.Species.Longevity *  effAge1))
                            age1 = a.Species.Longevity * effAge1;
                }
                if ((a.Species.Index == 1) && shrub2)
                {
                    if (age1 > (a.Species.Longevity * effAge2))
                        age1 = a.Species.Longevity * effAge2;
                }
                if ((a.Species.Index == 2) && shrub3)
                {
                    if (age1 > (a.Species.Longevity * effAge3))
                        age1 = a.Species.Longevity * effAge3;
                }
                if ((a.Species.Index == 3) && shrub4)
                {
                    if (age1 > (a.Species.Longevity * effAge4))
                        age1 = a.Species.Longevity * effAge4;
                }
                if ((a.Species.Index == 4) && shrub5)
                {
                    if (age1 > (a.Species.Longevity * effAge5))
                        age1 = a.Species.Longevity * effAge5;
                }
                if ((a.Species.Index == 5) && shrub6)
                {
                    if (age1 > (a.Species.Longevity * effAge6))
                        age1 = a.Species.Longevity * effAge6;
                }
                if ((b.Species.Index == 0) && shrub1)
                {
                    if (age2 > (b.Species.Longevity * effAge1))
                        age2 = b.Species.Longevity * effAge1;
                }
                if ((b.Species.Index == 1) && shrub2)
                {
                    if (age2 > (b.Species.Longevity * effAge2))
                        age2 = b.Species.Longevity * effAge2;
                }
                if ((b.Species.Index == 2) && shrub3)
                {
                    if (age2 > (b.Species.Longevity * effAge3))
                        age2 = b.Species.Longevity * effAge3;
                }
                if ((b.Species.Index == 3) && shrub4)
                {
                    if (age2 > (b.Species.Longevity * effAge4))
                        age2 = b.Species.Longevity * effAge4;
                } 
                if ((b.Species.Index == 4) && shrub5)
                {
                    if (age2 > (b.Species.Longevity * effAge5))
                        age2 = b.Species.Longevity * effAge5;
                }
                if ((b.Species.Index == 5) && shrub6)
                {
                    if (age2 > (b.Species.Longevity * effAge6))
                        age2 = b.Species.Longevity * effAge6;
                }
                if (age1 > age2)
                    return 1;
                if (age1 < age2)
                    return -1;
                else
                    return 0;
            }
        }

        private void species1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            string name = "Null"; 
            string longev = "0";
            string shadeTol = "0";
            string maturityAge = "0";
            string leafLong = "0";
            string decayRate = "0";
            string resproutProb = "0";
            string minVegAge = "0";
            string maxVegAge = "0";
            string ANPPMax = "0";
            string bioMax = "0";
            string mortShape = "0";
            string ANPPPower = "0";
            string propMort = "0";
            string ageMort = "0";
            string sec = "0";

            if (num1 == 1)
            {
                name = (tbSppName.Text); 
                longev = (tbLongevity.Text);
                shadeTol = (tbShadeTol.Text);
                maturityAge = (tbMatAge1.Text);
                leafLong = (tbLeaf.Text);
                decayRate = (tbDecay1.Text);
                resproutProb = (tbVegProb1.Text);
                minVegAge = (tbMinVegAge1.Text);
                maxVegAge = (tbMaxVegAge1.Text);
                ANPPMax = (tbANPPmax.Text);
                bioMax = (tbBioMax.Text);
                mortShape = (tbMortShape1.Text);
                ANPPPower = (tbPower1.Text);
                propMort = (tbMortMod1.Text);
                ageMort = (tbAgeMort1.Text);
                sec = (tbSEC.Text);
            }
            else if (num2 == 1)
            {
                name = (tbSppName2.Text); 
                longev = (tbLongevity2.Text);
                shadeTol = (tbShadeTol2.Text);
                maturityAge = (tbMatAge2.Text);
                leafLong = (tbLeaf2.Text);
                decayRate = (tbDecay2.Text);
                resproutProb = (tbVegProb2.Text);
                minVegAge = (tbMinVegAge2.Text);
                maxVegAge = (tbMaxVegAge2.Text);
                ANPPMax = (tbANPPmax2.Text);
                bioMax = (tbBioMax2.Text);
                mortShape = (tbMortShape2.Text);
                ANPPPower = (tbPower2.Text);
                propMort = (tbMortMod2.Text);
                ageMort = (tbAgeMort2.Text);
                sec = (tbSEC2.Text);
            }
            else if (num3 == 1)
            {
                name = (tbSppName3.Text); 
                longev = (tbLongevity3.Text);
                shadeTol = (tbShadeTol3.Text);
                maturityAge = (tbMatAge3.Text);
                leafLong = (tbLeaf3.Text);
                decayRate = (tbDecay3.Text);
                resproutProb = (tbVegProb3.Text);
                minVegAge = (tbMinVegAge3.Text);
                maxVegAge = (tbMaxVegAge3.Text);
                ANPPMax = (tbANPPmax3.Text);
                bioMax = (tbBioMax3.Text);
                mortShape = (tbMortShape3.Text);
                ANPPPower = (tbPower3.Text);
                propMort = (tbMortMod3.Text);
                ageMort = (tbAgeMort3.Text);
                sec = (tbSEC3.Text);
            }
            else if (num4 == 1)
            {
                name = (tbSppName4.Text);
                longev = (tbLongevity4.Text);
                shadeTol = (tbShadeTol4.Text);
                maturityAge = (tbMatAge4.Text);
                leafLong = (tbLeaf4.Text);
                decayRate = (tbDecay4.Text);
                resproutProb = (tbVegProb4.Text);
                minVegAge = (tbMinVegAge4.Text);
                maxVegAge = (tbMaxVegAge4.Text);
                ANPPMax = (tbANPPmax4.Text);
                bioMax = (tbBioMax4.Text);
                mortShape = (tbMortShape4.Text);
                ANPPPower = (tbPower4.Text);
                propMort = (tbMortMod4.Text);
                ageMort = (tbAgeMort4.Text);
                sec = (tbSEC4.Text);
            }
            else if (num5 == 1)
            {
                name = (tbSppName5.Text);
                longev = (tbLongevity5.Text);
                shadeTol = (tbShadeTol5.Text);
                maturityAge = (tbMatAge5.Text);
                leafLong = (tbLeaf5.Text);
                decayRate = (tbDecay5.Text);
                resproutProb = (tbVegProb5.Text);
                minVegAge = (tbMinVegAge5.Text);
                maxVegAge = (tbMaxVegAge5.Text);
                ANPPMax = (tbANPPmax5.Text);
                bioMax = (tbBioMax5.Text);
                mortShape = (tbMortShape5.Text);
                ANPPPower = (tbPower5.Text);
                propMort = (tbMortMod5.Text);
                ageMort = (tbAgeMort5.Text);
                sec = (tbSEC5.Text);
            }
            else if (num6 == 1)
            {
                name = (tbSppName6.Text);
                longev = (tbLongevity6.Text);
                shadeTol = (tbShadeTol6.Text);
                maturityAge = (tbMatAge6.Text);
                leafLong = (tbLeaf6.Text);
                decayRate = (tbDecay6.Text);
                resproutProb = (tbVegProb6.Text);
                minVegAge = (tbMinVegAge6.Text);
                maxVegAge = (tbMaxVegAge6.Text);
                ANPPMax = (tbANPPmax6.Text);
                bioMax = (tbBioMax6.Text);
                mortShape = (tbMortShape6.Text);
                ANPPPower = (tbPower6.Text);
                propMort = (tbMortMod6.Text);
                ageMort = (tbAgeMort6.Text);
                sec = (tbSEC6.Text);
            }
            List<string> paramList = new List<string>() ;
            paramList.Add(name); 
            paramList.Add(longev);
            paramList.Add(shadeTol);
            paramList.Add(maturityAge);
            paramList.Add(leafLong);
            paramList.Add(decayRate);
            paramList.Add(resproutProb);
            paramList.Add(minVegAge);
            paramList.Add(maxVegAge);
            paramList.Add(ANPPMax);
            paramList.Add(bioMax);
            paramList.Add(mortShape);
            paramList.Add(ANPPPower);
            paramList.Add(propMort);
            paramList.Add(ageMort);
            paramList.Add(sec);
            
            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }

        private void species2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            string name = "Null";
            string longev = "0";
            string shadeTol = "0";
            string maturityAge = "0";
            string leafLong = "0";
            string decayRate = "0";
            string resproutProb = "0";
            string minVegAge = "0";
            string maxVegAge = "0";
            string ANPPMax = "0";
            string bioMax = "0";
            string mortShape = "0";
            string ANPPPower = "0";
            string propMort = "0";
            string ageMort = "0";
            string sec = "0";

            if (num1 == 2)
            {
                name = (tbSppName.Text);
                longev = (tbLongevity.Text);
                shadeTol = (tbShadeTol.Text);
                maturityAge = (tbMatAge1.Text);
                leafLong = (tbLeaf.Text);
                decayRate = (tbDecay1.Text);
                resproutProb = (tbVegProb1.Text);
                minVegAge = (tbMinVegAge1.Text);
                maxVegAge = (tbMaxVegAge1.Text);
                ANPPMax = (tbANPPmax.Text);
                bioMax = (tbBioMax.Text);
                mortShape = (tbMortShape1.Text);
                ANPPPower = (tbPower1.Text);
                propMort = (tbMortMod1.Text);
                ageMort = (tbAgeMort1.Text);
                sec = (tbSEC.Text);
            }
            else if (num2 == 2)
            {
                name = (tbSppName2.Text);
                longev = (tbLongevity2.Text);
                shadeTol = (tbShadeTol2.Text);
                maturityAge = (tbMatAge2.Text);
                leafLong = (tbLeaf2.Text);
                decayRate = (tbDecay2.Text);
                resproutProb = (tbVegProb2.Text);
                minVegAge = (tbMinVegAge2.Text);
                maxVegAge = (tbMaxVegAge2.Text);
                ANPPMax = (tbANPPmax2.Text);
                bioMax = (tbBioMax2.Text);
                mortShape = (tbMortShape2.Text);
                ANPPPower = (tbPower2.Text);
                propMort = (tbMortMod2.Text);
                ageMort = (tbAgeMort2.Text);
                sec = (tbSEC2.Text);
            }
            else if (num3 == 2)
            {
                name = (tbSppName3.Text);
                longev = (tbLongevity3.Text);
                shadeTol = (tbShadeTol3.Text);
                maturityAge = (tbMatAge3.Text);
                leafLong = (tbLeaf3.Text);
                decayRate = (tbDecay3.Text);
                resproutProb = (tbVegProb3.Text);
                minVegAge = (tbMinVegAge3.Text);
                maxVegAge = (tbMaxVegAge3.Text);
                ANPPMax = (tbANPPmax3.Text);
                bioMax = (tbBioMax3.Text);
                mortShape = (tbMortShape3.Text);
                ANPPPower = (tbPower3.Text);
                propMort = (tbMortMod3.Text);
                ageMort = (tbAgeMort3.Text);
                sec = (tbSEC3.Text);
            }
            else if (num4 == 2)
            {
                name = (tbSppName4.Text);
                longev = (tbLongevity4.Text);
                shadeTol = (tbShadeTol4.Text);
                maturityAge = (tbMatAge4.Text);
                leafLong = (tbLeaf4.Text);
                decayRate = (tbDecay4.Text);
                resproutProb = (tbVegProb4.Text);
                minVegAge = (tbMinVegAge4.Text);
                maxVegAge = (tbMaxVegAge4.Text);
                ANPPMax = (tbANPPmax4.Text);
                bioMax = (tbBioMax4.Text);
                mortShape = (tbMortShape4.Text);
                ANPPPower = (tbPower4.Text);
                propMort = (tbMortMod4.Text);
                ageMort = (tbAgeMort4.Text);
                sec = (tbSEC4.Text);
            }
            else if (num5 == 2)
            {
                name = (tbSppName5.Text);
                longev = (tbLongevity5.Text);
                shadeTol = (tbShadeTol5.Text);
                maturityAge = (tbMatAge5.Text);
                leafLong = (tbLeaf5.Text);
                decayRate = (tbDecay5.Text);
                resproutProb = (tbVegProb5.Text);
                minVegAge = (tbMinVegAge5.Text);
                maxVegAge = (tbMaxVegAge5.Text);
                ANPPMax = (tbANPPmax5.Text);
                bioMax = (tbBioMax5.Text);
                mortShape = (tbMortShape5.Text);
                ANPPPower = (tbPower5.Text);
                propMort = (tbMortMod5.Text);
                ageMort = (tbAgeMort5.Text);
                sec = (tbSEC5.Text);
            }
            else if (num6 == 2)
            {
                name = (tbSppName6.Text);
                longev = (tbLongevity6.Text);
                shadeTol = (tbShadeTol6.Text);
                maturityAge = (tbMatAge6.Text);
                leafLong = (tbLeaf6.Text);
                decayRate = (tbDecay6.Text);
                resproutProb = (tbVegProb6.Text);
                minVegAge = (tbMinVegAge6.Text);
                maxVegAge = (tbMaxVegAge6.Text);
                ANPPMax = (tbANPPmax6.Text);
                bioMax = (tbBioMax6.Text);
                mortShape = (tbMortShape6.Text);
                ANPPPower = (tbPower6.Text);
                propMort = (tbMortMod6.Text);
                ageMort = (tbAgeMort6.Text);
                sec = (tbSEC6.Text);
            }
            List<string> paramList = new List<string>();
            paramList.Add(name);
            paramList.Add(longev);
            paramList.Add(shadeTol);
            paramList.Add(maturityAge);
            paramList.Add(leafLong);
            paramList.Add(decayRate);
            paramList.Add(resproutProb);
            paramList.Add(minVegAge);
            paramList.Add(maxVegAge);
            paramList.Add(ANPPMax);
            paramList.Add(bioMax);
            paramList.Add(mortShape);
            paramList.Add(ANPPPower);
            paramList.Add(propMort);
            paramList.Add(ageMort);
            paramList.Add(sec);

            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }
        private void species3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            string name = "Null";
            string longev = "0";
            string shadeTol = "0";
            string maturityAge = "0";
            string leafLong = "0";
            string decayRate = "0";
            string resproutProb = "0";
            string minVegAge = "0";
            string maxVegAge = "0";
            string ANPPMax = "0";
            string bioMax = "0";
            string mortShape = "0";
            string ANPPPower = "0";
            string propMort = "0";
            string ageMort = "0";
            string sec = "0";

            if (num1 == 3)
            {
                name = (tbSppName.Text);
                longev = (tbLongevity.Text);
                shadeTol = (tbShadeTol.Text);
                maturityAge = (tbMatAge1.Text);
                leafLong = (tbLeaf.Text);
                decayRate = (tbDecay1.Text);
                resproutProb = (tbVegProb1.Text);
                minVegAge = (tbMinVegAge1.Text);
                maxVegAge = (tbMaxVegAge1.Text);
                ANPPMax = (tbANPPmax.Text);
                bioMax = (tbBioMax.Text);
                mortShape = (tbMortShape1.Text);
                ANPPPower = (tbPower1.Text);
                propMort = (tbMortMod1.Text);
                ageMort = (tbAgeMort1.Text);
                sec = (tbSEC.Text);
            }
            else if (num2 == 3)
            {
                name = (tbSppName2.Text);
                longev = (tbLongevity2.Text);
                shadeTol = (tbShadeTol2.Text);
                maturityAge = (tbMatAge2.Text);
                leafLong = (tbLeaf2.Text);
                decayRate = (tbDecay2.Text);
                resproutProb = (tbVegProb2.Text);
                minVegAge = (tbMinVegAge2.Text);
                maxVegAge = (tbMaxVegAge2.Text);
                ANPPMax = (tbANPPmax2.Text);
                bioMax = (tbBioMax2.Text);
                mortShape = (tbMortShape2.Text);
                ANPPPower = (tbPower2.Text);
                propMort = (tbMortMod2.Text);
                ageMort = (tbAgeMort2.Text);
                sec = (tbSEC2.Text);
            }
            else if (num3 == 3)
            {
                name = (tbSppName3.Text);
                longev = (tbLongevity3.Text);
                shadeTol = (tbShadeTol3.Text);
                maturityAge = (tbMatAge3.Text);
                leafLong = (tbLeaf3.Text);
                decayRate = (tbDecay3.Text);
                resproutProb = (tbVegProb3.Text);
                minVegAge = (tbMinVegAge3.Text);
                maxVegAge = (tbMaxVegAge3.Text);
                ANPPMax = (tbANPPmax3.Text);
                bioMax = (tbBioMax3.Text);
                mortShape = (tbMortShape3.Text);
                ANPPPower = (tbPower3.Text);
                propMort = (tbMortMod3.Text);
                ageMort = (tbAgeMort3.Text);
                sec = (tbSEC3.Text);
            }
            else if (num4 == 3)
            {
                name = (tbSppName4.Text);
                longev = (tbLongevity4.Text);
                shadeTol = (tbShadeTol4.Text);
                maturityAge = (tbMatAge4.Text);
                leafLong = (tbLeaf4.Text);
                decayRate = (tbDecay4.Text);
                resproutProb = (tbVegProb4.Text);
                minVegAge = (tbMinVegAge4.Text);
                maxVegAge = (tbMaxVegAge4.Text);
                ANPPMax = (tbANPPmax4.Text);
                bioMax = (tbBioMax4.Text);
                mortShape = (tbMortShape4.Text);
                ANPPPower = (tbPower4.Text);
                propMort = (tbMortMod4.Text);
                ageMort = (tbAgeMort4.Text);
                sec = (tbSEC4.Text);
            }
            else if (num5 == 3)
            {
                name = (tbSppName5.Text);
                longev = (tbLongevity5.Text);
                shadeTol = (tbShadeTol5.Text);
                maturityAge = (tbMatAge5.Text);
                leafLong = (tbLeaf5.Text);
                decayRate = (tbDecay5.Text);
                resproutProb = (tbVegProb5.Text);
                minVegAge = (tbMinVegAge5.Text);
                maxVegAge = (tbMaxVegAge5.Text);
                ANPPMax = (tbANPPmax5.Text);
                bioMax = (tbBioMax5.Text);
                mortShape = (tbMortShape5.Text);
                ANPPPower = (tbPower5.Text);
                propMort = (tbMortMod5.Text);
                ageMort = (tbAgeMort5.Text);
                sec = (tbSEC5.Text);
            }
            else if (num6 == 3)
            {
                name = (tbSppName6.Text);
                longev = (tbLongevity6.Text);
                shadeTol = (tbShadeTol6.Text);
                maturityAge = (tbMatAge6.Text);
                leafLong = (tbLeaf6.Text);
                decayRate = (tbDecay6.Text);
                resproutProb = (tbVegProb6.Text);
                minVegAge = (tbMinVegAge6.Text);
                maxVegAge = (tbMaxVegAge6.Text);
                ANPPMax = (tbANPPmax6.Text);
                bioMax = (tbBioMax6.Text);
                mortShape = (tbMortShape6.Text);
                ANPPPower = (tbPower6.Text);
                propMort = (tbMortMod6.Text);
                ageMort = (tbAgeMort6.Text);
                sec = (tbSEC6.Text);
            }
            List<string> paramList = new List<string>();
            paramList.Add(name);
            paramList.Add(longev);
            paramList.Add(shadeTol);
            paramList.Add(maturityAge);
            paramList.Add(leafLong);
            paramList.Add(decayRate);
            paramList.Add(resproutProb);
            paramList.Add(minVegAge);
            paramList.Add(maxVegAge);
            paramList.Add(ANPPMax);
            paramList.Add(bioMax);
            paramList.Add(mortShape);
            paramList.Add(ANPPPower);
            paramList.Add(propMort);
            paramList.Add(ageMort);
            paramList.Add(sec);

            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }
        private void species4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            string name = "Null";
            string longev = "0";
            string shadeTol = "0";
            string maturityAge = "0";
            string leafLong = "0";
            string decayRate = "0";
            string resproutProb = "0";
            string minVegAge = "0";
            string maxVegAge = "0";
            string ANPPMax = "0";
            string bioMax = "0";
            string mortShape = "0";
            string ANPPPower = "0";
            string propMort = "0";
            string ageMort = "0";
            string sec = "0";

            if (num1 == 4)
            {
                name = (tbSppName.Text);
                longev = (tbLongevity.Text);
                shadeTol = (tbShadeTol.Text);
                maturityAge = (tbMatAge1.Text);
                leafLong = (tbLeaf.Text);
                decayRate = (tbDecay1.Text);
                resproutProb = (tbVegProb1.Text);
                minVegAge = (tbMinVegAge1.Text);
                maxVegAge = (tbMaxVegAge1.Text);
                ANPPMax = (tbANPPmax.Text);
                bioMax = (tbBioMax.Text);
                mortShape = (tbMortShape1.Text);
                ANPPPower = (tbPower1.Text);
                propMort = (tbMortMod1.Text);
                ageMort = (tbAgeMort1.Text);
                sec = (tbSEC.Text);
            }
            else if (num2 == 4)
            {
                name = (tbSppName2.Text);
                longev = (tbLongevity2.Text);
                shadeTol = (tbShadeTol2.Text);
                maturityAge = (tbMatAge2.Text);
                leafLong = (tbLeaf2.Text);
                decayRate = (tbDecay2.Text);
                resproutProb = (tbVegProb2.Text);
                minVegAge = (tbMinVegAge2.Text);
                maxVegAge = (tbMaxVegAge2.Text);
                ANPPMax = (tbANPPmax2.Text);
                bioMax = (tbBioMax2.Text);
                mortShape = (tbMortShape2.Text);
                ANPPPower = (tbPower2.Text);
                propMort = (tbMortMod2.Text);
                ageMort = (tbAgeMort2.Text);
                sec = (tbSEC2.Text);
            }
            else if (num3 == 4)
            {
                name = (tbSppName3.Text);
                longev = (tbLongevity3.Text);
                shadeTol = (tbShadeTol3.Text);
                maturityAge = (tbMatAge3.Text);
                leafLong = (tbLeaf3.Text);
                decayRate = (tbDecay3.Text);
                resproutProb = (tbVegProb3.Text);
                minVegAge = (tbMinVegAge3.Text);
                maxVegAge = (tbMaxVegAge3.Text);
                ANPPMax = (tbANPPmax3.Text);
                bioMax = (tbBioMax3.Text);
                mortShape = (tbMortShape3.Text);
                ANPPPower = (tbPower3.Text);
                propMort = (tbMortMod3.Text);
                ageMort = (tbAgeMort3.Text);
                sec = (tbSEC3.Text);
            }
            else if (num4 == 4)
            {
                name = (tbSppName4.Text);
                longev = (tbLongevity4.Text);
                shadeTol = (tbShadeTol4.Text);
                maturityAge = (tbMatAge4.Text);
                leafLong = (tbLeaf4.Text);
                decayRate = (tbDecay4.Text);
                resproutProb = (tbVegProb4.Text);
                minVegAge = (tbMinVegAge4.Text);
                maxVegAge = (tbMaxVegAge4.Text);
                ANPPMax = (tbANPPmax4.Text);
                bioMax = (tbBioMax4.Text);
                mortShape = (tbMortShape4.Text);
                ANPPPower = (tbPower4.Text);
                propMort = (tbMortMod4.Text);
                ageMort = (tbAgeMort4.Text);
                sec = (tbSEC4.Text);
            }
            else if (num5 == 4)
            {
                name = (tbSppName5.Text);
                longev = (tbLongevity5.Text);
                shadeTol = (tbShadeTol5.Text);
                maturityAge = (tbMatAge5.Text);
                leafLong = (tbLeaf5.Text);
                decayRate = (tbDecay5.Text);
                resproutProb = (tbVegProb5.Text);
                minVegAge = (tbMinVegAge5.Text);
                maxVegAge = (tbMaxVegAge5.Text);
                ANPPMax = (tbANPPmax5.Text);
                bioMax = (tbBioMax5.Text);
                mortShape = (tbMortShape5.Text);
                ANPPPower = (tbPower5.Text);
                propMort = (tbMortMod5.Text);
                ageMort = (tbAgeMort5.Text);
                sec = (tbSEC5.Text);
            }
            else if (num6 == 4)
            {
                name = (tbSppName6.Text);
                longev = (tbLongevity6.Text);
                shadeTol = (tbShadeTol6.Text);
                maturityAge = (tbMatAge6.Text);
                leafLong = (tbLeaf6.Text);
                decayRate = (tbDecay6.Text);
                resproutProb = (tbVegProb6.Text);
                minVegAge = (tbMinVegAge6.Text);
                maxVegAge = (tbMaxVegAge6.Text);
                ANPPMax = (tbANPPmax6.Text);
                bioMax = (tbBioMax6.Text);
                mortShape = (tbMortShape6.Text);
                ANPPPower = (tbPower6.Text);
                propMort = (tbMortMod6.Text);
                ageMort = (tbAgeMort6.Text);
                sec = (tbSEC6.Text);
            }
            List<string> paramList = new List<string>();
            paramList.Add(name);
            paramList.Add(longev);
            paramList.Add(shadeTol);
            paramList.Add(maturityAge);
            paramList.Add(leafLong);
            paramList.Add(decayRate);
            paramList.Add(resproutProb);
            paramList.Add(minVegAge);
            paramList.Add(maxVegAge);
            paramList.Add(ANPPMax);
            paramList.Add(bioMax);
            paramList.Add(mortShape);
            paramList.Add(ANPPPower);
            paramList.Add(propMort);
            paramList.Add(ageMort);
            paramList.Add(sec);

            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }
        private void species5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            string name = "Null";
            string longev = "0";
            string shadeTol = "0";
            string maturityAge = "0";
            string leafLong = "0";
            string decayRate = "0";
            string resproutProb = "0";
            string minVegAge = "0";
            string maxVegAge = "0";
            string ANPPMax = "0";
            string bioMax = "0";
            string mortShape = "0";
            string ANPPPower = "0";
            string propMort = "0";
            string ageMort = "0";
            string sec = "0";

            if (num1 == 5)
            {
                name = (tbSppName.Text);
                longev = (tbLongevity.Text);
                shadeTol = (tbShadeTol.Text);
                maturityAge = (tbMatAge1.Text);
                leafLong = (tbLeaf.Text);
                decayRate = (tbDecay1.Text);
                resproutProb = (tbVegProb1.Text);
                minVegAge = (tbMinVegAge1.Text);
                maxVegAge = (tbMaxVegAge1.Text);
                ANPPMax = (tbANPPmax.Text);
                bioMax = (tbBioMax.Text);
                mortShape = (tbMortShape1.Text);
                ANPPPower = (tbPower1.Text);
                propMort = (tbMortMod1.Text);
                ageMort = (tbAgeMort1.Text);
                sec = (tbSEC.Text);
            }
            else if (num2 == 5)
            {
                name = (tbSppName2.Text);
                longev = (tbLongevity2.Text);
                shadeTol = (tbShadeTol2.Text);
                maturityAge = (tbMatAge2.Text);
                leafLong = (tbLeaf2.Text);
                decayRate = (tbDecay2.Text);
                resproutProb = (tbVegProb2.Text);
                minVegAge = (tbMinVegAge2.Text);
                maxVegAge = (tbMaxVegAge2.Text);
                ANPPMax = (tbANPPmax2.Text);
                bioMax = (tbBioMax2.Text);
                mortShape = (tbMortShape2.Text);
                ANPPPower = (tbPower2.Text);
                propMort = (tbMortMod2.Text);
                ageMort = (tbAgeMort2.Text);
                sec = (tbSEC2.Text);
            }
            else if (num3 == 5)
            {
                name = (tbSppName3.Text);
                longev = (tbLongevity3.Text);
                shadeTol = (tbShadeTol3.Text);
                maturityAge = (tbMatAge3.Text);
                leafLong = (tbLeaf3.Text);
                decayRate = (tbDecay3.Text);
                resproutProb = (tbVegProb3.Text);
                minVegAge = (tbMinVegAge3.Text);
                maxVegAge = (tbMaxVegAge3.Text);
                ANPPMax = (tbANPPmax3.Text);
                bioMax = (tbBioMax3.Text);
                mortShape = (tbMortShape3.Text);
                ANPPPower = (tbPower3.Text);
                propMort = (tbMortMod3.Text);
                ageMort = (tbAgeMort3.Text);
                sec = (tbSEC3.Text);
            }
            else if (num4 == 5)
            {
                name = (tbSppName4.Text);
                longev = (tbLongevity4.Text);
                shadeTol = (tbShadeTol4.Text);
                maturityAge = (tbMatAge4.Text);
                leafLong = (tbLeaf4.Text);
                decayRate = (tbDecay4.Text);
                resproutProb = (tbVegProb4.Text);
                minVegAge = (tbMinVegAge4.Text);
                maxVegAge = (tbMaxVegAge4.Text);
                ANPPMax = (tbANPPmax4.Text);
                bioMax = (tbBioMax4.Text);
                mortShape = (tbMortShape4.Text);
                ANPPPower = (tbPower4.Text);
                propMort = (tbMortMod4.Text);
                ageMort = (tbAgeMort4.Text);
                sec = (tbSEC4.Text);
            }
            else if (num5 == 5)
            {
                name = (tbSppName5.Text);
                longev = (tbLongevity5.Text);
                shadeTol = (tbShadeTol5.Text);
                maturityAge = (tbMatAge5.Text);
                leafLong = (tbLeaf5.Text);
                decayRate = (tbDecay5.Text);
                resproutProb = (tbVegProb5.Text);
                minVegAge = (tbMinVegAge5.Text);
                maxVegAge = (tbMaxVegAge5.Text);
                ANPPMax = (tbANPPmax5.Text);
                bioMax = (tbBioMax5.Text);
                mortShape = (tbMortShape5.Text);
                ANPPPower = (tbPower5.Text);
                propMort = (tbMortMod5.Text);
                ageMort = (tbAgeMort5.Text);
                sec = (tbSEC5.Text);
            }
            else if (num6 == 5)
            {
                name = (tbSppName6.Text);
                longev = (tbLongevity6.Text);
                shadeTol = (tbShadeTol6.Text);
                maturityAge = (tbMatAge6.Text);
                leafLong = (tbLeaf6.Text);
                decayRate = (tbDecay6.Text);
                resproutProb = (tbVegProb6.Text);
                minVegAge = (tbMinVegAge6.Text);
                maxVegAge = (tbMaxVegAge6.Text);
                ANPPMax = (tbANPPmax6.Text);
                bioMax = (tbBioMax6.Text);
                mortShape = (tbMortShape6.Text);
                ANPPPower = (tbPower6.Text);
                propMort = (tbMortMod6.Text);
                ageMort = (tbAgeMort6.Text);
                sec = (tbSEC6.Text);
            }
            List<string> paramList = new List<string>();
            paramList.Add(name);
            paramList.Add(longev);
            paramList.Add(shadeTol);
            paramList.Add(maturityAge);
            paramList.Add(leafLong);
            paramList.Add(decayRate);
            paramList.Add(resproutProb);
            paramList.Add(minVegAge);
            paramList.Add(maxVegAge);
            paramList.Add(ANPPMax);
            paramList.Add(bioMax);
            paramList.Add(mortShape);
            paramList.Add(ANPPPower);
            paramList.Add(propMort);
            paramList.Add(ageMort);
            paramList.Add(sec);

            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }
        private void species6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            string name = "Null";
            string longev = "0";
            string shadeTol = "0";
            string maturityAge = "0";
            string leafLong = "0";
            string decayRate = "0";
            string resproutProb = "0";
            string minVegAge = "0";
            string maxVegAge = "0";
            string ANPPMax = "0";
            string bioMax = "0";
            string mortShape = "0";
            string ANPPPower = "0";
            string propMort = "0";
            string ageMort = "0";
            string sec = "0";

            if (num1 == 6)
            {
                name = (tbSppName.Text);
                longev = (tbLongevity.Text);
                shadeTol = (tbShadeTol.Text);
                maturityAge = (tbMatAge1.Text);
                leafLong = (tbLeaf.Text);
                decayRate = (tbDecay1.Text);
                resproutProb = (tbVegProb1.Text);
                minVegAge = (tbMinVegAge1.Text);
                maxVegAge = (tbMaxVegAge1.Text);
                ANPPMax = (tbANPPmax.Text);
                bioMax = (tbBioMax.Text);
                mortShape = (tbMortShape1.Text);
                ANPPPower = (tbPower1.Text);
                propMort = (tbMortMod1.Text);
                ageMort = (tbAgeMort1.Text);
                sec = (tbSEC.Text);
            }
            else if (num2 == 6)
            {
                name = (tbSppName2.Text);
                longev = (tbLongevity2.Text);
                shadeTol = (tbShadeTol2.Text);
                maturityAge = (tbMatAge2.Text);
                leafLong = (tbLeaf2.Text);
                decayRate = (tbDecay2.Text);
                resproutProb = (tbVegProb2.Text);
                minVegAge = (tbMinVegAge2.Text);
                maxVegAge = (tbMaxVegAge2.Text);
                ANPPMax = (tbANPPmax2.Text);
                bioMax = (tbBioMax2.Text);
                mortShape = (tbMortShape2.Text);
                ANPPPower = (tbPower2.Text);
                propMort = (tbMortMod2.Text);
                ageMort = (tbAgeMort2.Text);
                sec = (tbSEC2.Text);
            }
            else if (num3 == 6)
            {
                name = (tbSppName3.Text);
                longev = (tbLongevity3.Text);
                shadeTol = (tbShadeTol3.Text);
                maturityAge = (tbMatAge3.Text);
                leafLong = (tbLeaf3.Text);
                decayRate = (tbDecay3.Text);
                resproutProb = (tbVegProb3.Text);
                minVegAge = (tbMinVegAge3.Text);
                maxVegAge = (tbMaxVegAge3.Text);
                ANPPMax = (tbANPPmax3.Text);
                bioMax = (tbBioMax3.Text);
                mortShape = (tbMortShape3.Text);
                ANPPPower = (tbPower3.Text);
                propMort = (tbMortMod3.Text);
                ageMort = (tbAgeMort3.Text);
                sec = (tbSEC3.Text);
            }
            else if (num4 == 6)
            {
                name = (tbSppName4.Text);
                longev = (tbLongevity4.Text);
                shadeTol = (tbShadeTol4.Text);
                maturityAge = (tbMatAge4.Text);
                leafLong = (tbLeaf4.Text);
                decayRate = (tbDecay4.Text);
                resproutProb = (tbVegProb4.Text);
                minVegAge = (tbMinVegAge4.Text);
                maxVegAge = (tbMaxVegAge4.Text);
                ANPPMax = (tbANPPmax4.Text);
                bioMax = (tbBioMax4.Text);
                mortShape = (tbMortShape4.Text);
                ANPPPower = (tbPower4.Text);
                propMort = (tbMortMod4.Text);
                ageMort = (tbAgeMort4.Text);
                sec = (tbSEC4.Text);
            }
            else if (num5 == 6)
            {
                name = (tbSppName5.Text);
                longev = (tbLongevity5.Text);
                shadeTol = (tbShadeTol5.Text);
                maturityAge = (tbMatAge5.Text);
                leafLong = (tbLeaf5.Text);
                decayRate = (tbDecay5.Text);
                resproutProb = (tbVegProb5.Text);
                minVegAge = (tbMinVegAge5.Text);
                maxVegAge = (tbMaxVegAge5.Text);
                ANPPMax = (tbANPPmax5.Text);
                bioMax = (tbBioMax5.Text);
                mortShape = (tbMortShape5.Text);
                ANPPPower = (tbPower5.Text);
                propMort = (tbMortMod5.Text);
                ageMort = (tbAgeMort5.Text);
                sec = (tbSEC5.Text);
            }
            else if (num6 == 6)
            {
                name = (tbSppName6.Text);
                longev = (tbLongevity6.Text);
                shadeTol = (tbShadeTol6.Text);
                maturityAge = (tbMatAge6.Text);
                leafLong = (tbLeaf6.Text);
                decayRate = (tbDecay6.Text);
                resproutProb = (tbVegProb6.Text);
                minVegAge = (tbMinVegAge6.Text);
                maxVegAge = (tbMaxVegAge6.Text);
                ANPPMax = (tbANPPmax6.Text);
                bioMax = (tbBioMax6.Text);
                mortShape = (tbMortShape6.Text);
                ANPPPower = (tbPower6.Text);
                propMort = (tbMortMod6.Text);
                ageMort = (tbAgeMort6.Text);
                sec = (tbSEC6.Text);
            }
            List<string> paramList = new List<string>();
            paramList.Add(name);
            paramList.Add(longev);
            paramList.Add(shadeTol);
            paramList.Add(maturityAge);
            paramList.Add(leafLong);
            paramList.Add(decayRate);
            paramList.Add(resproutProb);
            paramList.Add(minVegAge);
            paramList.Add(maxVegAge);
            paramList.Add(ANPPMax);
            paramList.Add(bioMax);
            paramList.Add(mortShape);
            paramList.Add(ANPPPower);
            paramList.Add(propMort);
            paramList.Add(ageMort);
            paramList.Add(sec);

            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }
        private void load1ParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);
                
                
            } while (!fileReader.EndOfStream);

            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            if (num1 == 1)
            {
                tbSppName.Text = paramList[0];
                tbLongevity.Text = paramList[1];
                tbShadeTol.Text = paramList[2];
                tbAgeMort1.Text = paramList[3];
                tbLeaf.Text = paramList[4];
                tbDecay1.Text = paramList[5];
                tbVegProb1.Text = paramList[6];
                tbMinVegAge1.Text = paramList[7];
                tbMaxVegAge1.Text = paramList[8];
                tbANPPmax.Text = paramList[9];
                tbBioMax.Text = paramList[10];
                tbMortShape1.Text = paramList[11];
                tbPower1.Text = paramList[12];
                tbMortMod1.Text = paramList[13];
                tbAgeMort1.Text = paramList[14];
                tbSEC.Text = paramList[15];
            }
            else if (num2 == 1)
            {
                tbSppName2.Text = paramList[0];
                tbLongevity2.Text = paramList[1];
                tbShadeTol2.Text = paramList[2];
                tbAgeMort2.Text = paramList[3];
                tbLeaf2.Text = paramList[4];
                tbDecay2.Text = paramList[5];
                tbVegProb2.Text = paramList[6];
                tbMinVegAge2.Text = paramList[7];
                tbMaxVegAge2.Text = paramList[8];
                tbANPPmax2.Text = paramList[9];
                tbBioMax2.Text = paramList[10];
                tbMortShape2.Text = paramList[11];
                tbPower2.Text = paramList[12];
                tbMortMod2.Text = paramList[13];
                tbAgeMort2.Text = paramList[14];
                tbSEC2.Text = paramList[15];
            }
            else if (num3 == 1)
            {
                tbSppName3.Text = paramList[0];
                tbLongevity3.Text = paramList[1];
                tbShadeTol3.Text = paramList[2];
                tbAgeMort3.Text = paramList[3];
                tbLeaf3.Text = paramList[4];
                tbDecay3.Text = paramList[5];
                tbVegProb3.Text = paramList[6];
                tbMinVegAge3.Text = paramList[7];
                tbMaxVegAge3.Text = paramList[8];
                tbANPPmax3.Text = paramList[9];
                tbBioMax3.Text = paramList[10];
                tbMortShape3.Text = paramList[11];
                tbPower3.Text = paramList[12];
                tbMortMod3.Text = paramList[13];
                tbAgeMort3.Text = paramList[14];
                tbSEC3.Text = paramList[15];
            }
            else if (num4 == 1)
            {
                tbSppName4.Text = paramList[0];
                tbLongevity4.Text = paramList[1];
                tbShadeTol4.Text = paramList[2];
                tbAgeMort4.Text = paramList[3];
                tbLeaf4.Text = paramList[4];
                tbDecay4.Text = paramList[5];
                tbVegProb4.Text = paramList[6];
                tbMinVegAge4.Text = paramList[7];
                tbMaxVegAge4.Text = paramList[8];
                tbANPPmax4.Text = paramList[9];
                tbBioMax4.Text = paramList[10];
                tbMortShape4.Text = paramList[11];
                tbPower4.Text = paramList[12];
                tbMortMod4.Text = paramList[13];
                tbAgeMort4.Text = paramList[14];
                tbSEC4.Text = paramList[15];
            }
            else if (num5 == 1)
            {
                tbSppName5.Text = paramList[0];
                tbLongevity5.Text = paramList[1];
                tbShadeTol5.Text = paramList[2];
                tbAgeMort5.Text = paramList[3];
                tbLeaf5.Text = paramList[4];
                tbDecay5.Text = paramList[5];
                tbVegProb5.Text = paramList[6];
                tbMinVegAge5.Text = paramList[7];
                tbMaxVegAge5.Text = paramList[8];
                tbANPPmax5.Text = paramList[9];
                tbBioMax5.Text = paramList[10];
                tbMortShape5.Text = paramList[11];
                tbPower5.Text = paramList[12];
                tbMortMod5.Text = paramList[13];
                tbAgeMort5.Text = paramList[14];
                tbSEC5.Text = paramList[15];
            }
            else if (num6 == 1)
            {
                tbSppName6.Text = paramList[0];
                tbLongevity6.Text = paramList[1];
                tbShadeTol6.Text = paramList[2];
                tbAgeMort6.Text = paramList[3];
                tbLeaf6.Text = paramList[4];
                tbDecay6.Text = paramList[5];
                tbVegProb6.Text = paramList[6];
                tbMinVegAge6.Text = paramList[7];
                tbMaxVegAge6.Text = paramList[8];
                tbANPPmax6.Text = paramList[9];
                tbBioMax6.Text = paramList[10];
                tbMortShape6.Text = paramList[11];
                tbPower6.Text = paramList[12];
                tbMortMod6.Text = paramList[13];
                tbAgeMort6.Text = paramList[14];
                tbSEC6.Text = paramList[15];
            }
            
        }

        private void load2ParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);


            } while (!fileReader.EndOfStream);

            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            if (num1 == 2)
            {
                tbSppName.Text = paramList[0];
                tbLongevity.Text = paramList[1];
                tbShadeTol.Text = paramList[2];
                tbAgeMort1.Text = paramList[3];
                tbLeaf.Text = paramList[4];
                tbDecay1.Text = paramList[5];
                tbVegProb1.Text = paramList[6];
                tbMinVegAge1.Text = paramList[7];
                tbMaxVegAge1.Text = paramList[8];
                tbANPPmax.Text = paramList[9];
                tbBioMax.Text = paramList[10];
                tbMortShape1.Text = paramList[11];
                tbPower1.Text = paramList[12];
                tbMortMod1.Text = paramList[13];
                tbAgeMort1.Text = paramList[14];
                tbSEC.Text = paramList[15];
            }
            else if (num2 == 2)
            {
                tbSppName2.Text = paramList[0];
                tbLongevity2.Text = paramList[1];
                tbShadeTol2.Text = paramList[2];
                tbAgeMort2.Text = paramList[3];
                tbLeaf2.Text = paramList[4];
                tbDecay2.Text = paramList[5];
                tbVegProb2.Text = paramList[6];
                tbMinVegAge2.Text = paramList[7];
                tbMaxVegAge2.Text = paramList[8];
                tbANPPmax2.Text = paramList[9];
                tbBioMax2.Text = paramList[10];
                tbMortShape2.Text = paramList[11];
                tbPower2.Text = paramList[12];
                tbMortMod2.Text = paramList[13];
                tbAgeMort2.Text = paramList[14];
                tbSEC2.Text = paramList[15];
            }
            else if (num3 == 2)
            {
                tbSppName3.Text = paramList[0];
                tbLongevity3.Text = paramList[1];
                tbShadeTol3.Text = paramList[2];
                tbAgeMort3.Text = paramList[3];
                tbLeaf3.Text = paramList[4];
                tbDecay3.Text = paramList[5];
                tbVegProb3.Text = paramList[6];
                tbMinVegAge3.Text = paramList[7];
                tbMaxVegAge3.Text = paramList[8];
                tbANPPmax3.Text = paramList[9];
                tbBioMax3.Text = paramList[10];
                tbMortShape3.Text = paramList[11];
                tbPower3.Text = paramList[12];
                tbMortMod3.Text = paramList[13];
                tbAgeMort3.Text = paramList[14];
                tbSEC3.Text = paramList[15];
            }
            else if (num4 == 2)
            {
                tbSppName4.Text = paramList[0];
                tbLongevity4.Text = paramList[1];
                tbShadeTol4.Text = paramList[2];
                tbAgeMort4.Text = paramList[3];
                tbLeaf4.Text = paramList[4];
                tbDecay4.Text = paramList[5];
                tbVegProb4.Text = paramList[6];
                tbMinVegAge4.Text = paramList[7];
                tbMaxVegAge4.Text = paramList[8];
                tbANPPmax4.Text = paramList[9];
                tbBioMax4.Text = paramList[10];
                tbMortShape4.Text = paramList[11];
                tbPower4.Text = paramList[12];
                tbMortMod4.Text = paramList[13];
                tbAgeMort4.Text = paramList[14];
                tbSEC4.Text = paramList[15];
            }
            else if (num5 == 2)
            {
                tbSppName5.Text = paramList[0];
                tbLongevity5.Text = paramList[1];
                tbShadeTol5.Text = paramList[2];
                tbAgeMort5.Text = paramList[3];
                tbLeaf5.Text = paramList[4];
                tbDecay5.Text = paramList[5];
                tbVegProb5.Text = paramList[6];
                tbMinVegAge5.Text = paramList[7];
                tbMaxVegAge5.Text = paramList[8];
                tbANPPmax5.Text = paramList[9];
                tbBioMax5.Text = paramList[10];
                tbMortShape5.Text = paramList[11];
                tbPower5.Text = paramList[12];
                tbMortMod5.Text = paramList[13];
                tbAgeMort5.Text = paramList[14];
                tbSEC5.Text = paramList[15];
            }
            else if (num6 == 2)
            {
                tbSppName6.Text = paramList[0];
                tbLongevity6.Text = paramList[1];
                tbShadeTol6.Text = paramList[2];
                tbAgeMort6.Text = paramList[3];
                tbLeaf6.Text = paramList[4];
                tbDecay6.Text = paramList[5];
                tbVegProb6.Text = paramList[6];
                tbMinVegAge6.Text = paramList[7];
                tbMaxVegAge6.Text = paramList[8];
                tbANPPmax6.Text = paramList[9];
                tbBioMax6.Text = paramList[10];
                tbMortShape6.Text = paramList[11];
                tbPower6.Text = paramList[12];
                tbMortMod6.Text = paramList[13];
                tbAgeMort6.Text = paramList[14];
                tbSEC6.Text = paramList[15];
            }

        }
        private void load3ParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);


            } while (!fileReader.EndOfStream);

            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            if (num1 == 3)
            {
                tbSppName.Text = paramList[0];
                tbLongevity.Text = paramList[1];
                tbShadeTol.Text = paramList[2];
                tbAgeMort1.Text = paramList[3];
                tbLeaf.Text = paramList[4];
                tbDecay1.Text = paramList[5];
                tbVegProb1.Text = paramList[6];
                tbMinVegAge1.Text = paramList[7];
                tbMaxVegAge1.Text = paramList[8];
                tbANPPmax.Text = paramList[9];
                tbBioMax.Text = paramList[10];
                tbMortShape1.Text = paramList[11];
                tbPower1.Text = paramList[12];
                tbMortMod1.Text = paramList[13];
                tbAgeMort1.Text = paramList[14];
                tbSEC.Text = paramList[15];
            }
            else if (num2 == 3)
            {
                tbSppName2.Text = paramList[0];
                tbLongevity2.Text = paramList[1];
                tbShadeTol2.Text = paramList[2];
                tbAgeMort2.Text = paramList[3];
                tbLeaf2.Text = paramList[4];
                tbDecay2.Text = paramList[5];
                tbVegProb2.Text = paramList[6];
                tbMinVegAge2.Text = paramList[7];
                tbMaxVegAge2.Text = paramList[8];
                tbANPPmax2.Text = paramList[9];
                tbBioMax2.Text = paramList[10];
                tbMortShape2.Text = paramList[11];
                tbPower2.Text = paramList[12];
                tbMortMod2.Text = paramList[13];
                tbAgeMort2.Text = paramList[14];
                tbSEC2.Text = paramList[15];
            }
            else if (num3 == 3)
            {
                tbSppName3.Text = paramList[0];
                tbLongevity3.Text = paramList[1];
                tbShadeTol3.Text = paramList[2];
                tbAgeMort3.Text = paramList[3];
                tbLeaf3.Text = paramList[4];
                tbDecay3.Text = paramList[5];
                tbVegProb3.Text = paramList[6];
                tbMinVegAge3.Text = paramList[7];
                tbMaxVegAge3.Text = paramList[8];
                tbANPPmax3.Text = paramList[9];
                tbBioMax3.Text = paramList[10];
                tbMortShape3.Text = paramList[11];
                tbPower3.Text = paramList[12];
                tbMortMod3.Text = paramList[13];
                tbAgeMort3.Text = paramList[14];
                tbSEC3.Text = paramList[15];
            }
            else if (num4 == 3)
            {
                tbSppName4.Text = paramList[0];
                tbLongevity4.Text = paramList[1];
                tbShadeTol4.Text = paramList[2];
                tbAgeMort4.Text = paramList[3];
                tbLeaf4.Text = paramList[4];
                tbDecay4.Text = paramList[5];
                tbVegProb4.Text = paramList[6];
                tbMinVegAge4.Text = paramList[7];
                tbMaxVegAge4.Text = paramList[8];
                tbANPPmax4.Text = paramList[9];
                tbBioMax4.Text = paramList[10];
                tbMortShape4.Text = paramList[11];
                tbPower4.Text = paramList[12];
                tbMortMod4.Text = paramList[13];
                tbAgeMort4.Text = paramList[14];
                tbSEC4.Text = paramList[15];
            }
            else if (num5 == 3)
            {
                tbSppName5.Text = paramList[0];
                tbLongevity5.Text = paramList[1];
                tbShadeTol5.Text = paramList[2];
                tbAgeMort5.Text = paramList[3];
                tbLeaf5.Text = paramList[4];
                tbDecay5.Text = paramList[5];
                tbVegProb5.Text = paramList[6];
                tbMinVegAge5.Text = paramList[7];
                tbMaxVegAge5.Text = paramList[8];
                tbANPPmax5.Text = paramList[9];
                tbBioMax5.Text = paramList[10];
                tbMortShape5.Text = paramList[11];
                tbPower5.Text = paramList[12];
                tbMortMod5.Text = paramList[13];
                tbAgeMort5.Text = paramList[14];
                tbSEC5.Text = paramList[15];
            }
            else if (num6 == 3)
            {
                tbSppName6.Text = paramList[0];
                tbLongevity6.Text = paramList[1];
                tbShadeTol6.Text = paramList[2];
                tbAgeMort6.Text = paramList[3];
                tbLeaf6.Text = paramList[4];
                tbDecay6.Text = paramList[5];
                tbVegProb6.Text = paramList[6];
                tbMinVegAge6.Text = paramList[7];
                tbMaxVegAge6.Text = paramList[8];
                tbANPPmax6.Text = paramList[9];
                tbBioMax6.Text = paramList[10];
                tbMortShape6.Text = paramList[11];
                tbPower6.Text = paramList[12];
                tbMortMod6.Text = paramList[13];
                tbAgeMort6.Text = paramList[14];
                tbSEC6.Text = paramList[15];
            }

        }
        private void load4ParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);


            } while (!fileReader.EndOfStream);

            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            if (num1 == 4)
            {
                tbSppName.Text = paramList[0];
                tbLongevity.Text = paramList[1];
                tbShadeTol.Text = paramList[2];
                tbAgeMort1.Text = paramList[3];
                tbLeaf.Text = paramList[4];
                tbDecay1.Text = paramList[5];
                tbVegProb1.Text = paramList[6];
                tbMinVegAge1.Text = paramList[7];
                tbMaxVegAge1.Text = paramList[8];
                tbANPPmax.Text = paramList[9];
                tbBioMax.Text = paramList[10];
                tbMortShape1.Text = paramList[11];
                tbPower1.Text = paramList[12];
                tbMortMod1.Text = paramList[13];
                tbAgeMort1.Text = paramList[14];
                tbSEC.Text = paramList[15];
            }
            else if (num2 == 4)
            {
                tbSppName2.Text = paramList[0];
                tbLongevity2.Text = paramList[1];
                tbShadeTol2.Text = paramList[2];
                tbAgeMort2.Text = paramList[3];
                tbLeaf2.Text = paramList[4];
                tbDecay2.Text = paramList[5];
                tbVegProb2.Text = paramList[6];
                tbMinVegAge2.Text = paramList[7];
                tbMaxVegAge2.Text = paramList[8];
                tbANPPmax2.Text = paramList[9];
                tbBioMax2.Text = paramList[10];
                tbMortShape2.Text = paramList[11];
                tbPower2.Text = paramList[12];
                tbMortMod2.Text = paramList[13];
                tbAgeMort2.Text = paramList[14];
                tbSEC2.Text = paramList[15];
            }
            else if (num3 == 4)
            {
                tbSppName3.Text = paramList[0];
                tbLongevity3.Text = paramList[1];
                tbShadeTol3.Text = paramList[2];
                tbAgeMort3.Text = paramList[3];
                tbLeaf3.Text = paramList[4];
                tbDecay3.Text = paramList[5];
                tbVegProb3.Text = paramList[6];
                tbMinVegAge3.Text = paramList[7];
                tbMaxVegAge3.Text = paramList[8];
                tbANPPmax3.Text = paramList[9];
                tbBioMax3.Text = paramList[10];
                tbMortShape3.Text = paramList[11];
                tbPower3.Text = paramList[12];
                tbMortMod3.Text = paramList[13];
                tbAgeMort3.Text = paramList[14];
                tbSEC3.Text = paramList[15];
            }
            else if (num4 == 4)
            {
                tbSppName4.Text = paramList[0];
                tbLongevity4.Text = paramList[1];
                tbShadeTol4.Text = paramList[2];
                tbAgeMort4.Text = paramList[3];
                tbLeaf4.Text = paramList[4];
                tbDecay4.Text = paramList[5];
                tbVegProb4.Text = paramList[6];
                tbMinVegAge4.Text = paramList[7];
                tbMaxVegAge4.Text = paramList[8];
                tbANPPmax4.Text = paramList[9];
                tbBioMax4.Text = paramList[10];
                tbMortShape4.Text = paramList[11];
                tbPower4.Text = paramList[12];
                tbMortMod4.Text = paramList[13];
                tbAgeMort4.Text = paramList[14];
                tbSEC4.Text = paramList[15];
            }
            else if (num5 == 4)
            {
                tbSppName5.Text = paramList[0];
                tbLongevity5.Text = paramList[1];
                tbShadeTol5.Text = paramList[2];
                tbAgeMort5.Text = paramList[3];
                tbLeaf5.Text = paramList[4];
                tbDecay5.Text = paramList[5];
                tbVegProb5.Text = paramList[6];
                tbMinVegAge5.Text = paramList[7];
                tbMaxVegAge5.Text = paramList[8];
                tbANPPmax5.Text = paramList[9];
                tbBioMax5.Text = paramList[10];
                tbMortShape5.Text = paramList[11];
                tbPower5.Text = paramList[12];
                tbMortMod5.Text = paramList[13];
                tbAgeMort5.Text = paramList[14];
                tbSEC5.Text = paramList[15];
            }
            else if (num6 == 4)
            {
                tbSppName6.Text = paramList[0];
                tbLongevity6.Text = paramList[1];
                tbShadeTol6.Text = paramList[2];
                tbAgeMort6.Text = paramList[3];
                tbLeaf6.Text = paramList[4];
                tbDecay6.Text = paramList[5];
                tbVegProb6.Text = paramList[6];
                tbMinVegAge6.Text = paramList[7];
                tbMaxVegAge6.Text = paramList[8];
                tbANPPmax6.Text = paramList[9];
                tbBioMax6.Text = paramList[10];
                tbMortShape6.Text = paramList[11];
                tbPower6.Text = paramList[12];
                tbMortMod6.Text = paramList[13];
                tbAgeMort6.Text = paramList[14];
                tbSEC6.Text = paramList[15];
            }

        }
        private void load5ParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);


            } while (!fileReader.EndOfStream);

            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            if (num1 == 5)
            {
                tbSppName.Text = paramList[0];
                tbLongevity.Text = paramList[1];
                tbShadeTol.Text = paramList[2];
                tbAgeMort1.Text = paramList[3];
                tbLeaf.Text = paramList[4];
                tbDecay1.Text = paramList[5];
                tbVegProb1.Text = paramList[6];
                tbMinVegAge1.Text = paramList[7];
                tbMaxVegAge1.Text = paramList[8];
                tbANPPmax.Text = paramList[9];
                tbBioMax.Text = paramList[10];
                tbMortShape1.Text = paramList[11];
                tbPower1.Text = paramList[12];
                tbMortMod1.Text = paramList[13];
                tbAgeMort1.Text = paramList[14];
                tbSEC.Text = paramList[15];
            }
            else if (num2 == 5)
            {
                tbSppName2.Text = paramList[0];
                tbLongevity2.Text = paramList[1];
                tbShadeTol2.Text = paramList[2];
                tbAgeMort2.Text = paramList[3];
                tbLeaf2.Text = paramList[4];
                tbDecay2.Text = paramList[5];
                tbVegProb2.Text = paramList[6];
                tbMinVegAge2.Text = paramList[7];
                tbMaxVegAge2.Text = paramList[8];
                tbANPPmax2.Text = paramList[9];
                tbBioMax2.Text = paramList[10];
                tbMortShape2.Text = paramList[11];
                tbPower2.Text = paramList[12];
                tbMortMod2.Text = paramList[13];
                tbAgeMort2.Text = paramList[14];
                tbSEC2.Text = paramList[15];
            }
            else if (num3 == 5)
            {
                tbSppName3.Text = paramList[0];
                tbLongevity3.Text = paramList[1];
                tbShadeTol3.Text = paramList[2];
                tbAgeMort3.Text = paramList[3];
                tbLeaf3.Text = paramList[4];
                tbDecay3.Text = paramList[5];
                tbVegProb3.Text = paramList[6];
                tbMinVegAge3.Text = paramList[7];
                tbMaxVegAge3.Text = paramList[8];
                tbANPPmax3.Text = paramList[9];
                tbBioMax3.Text = paramList[10];
                tbMortShape3.Text = paramList[11];
                tbPower3.Text = paramList[12];
                tbMortMod3.Text = paramList[13];
                tbAgeMort3.Text = paramList[14];
                tbSEC3.Text = paramList[15];
            }
            else if (num4 == 5)
            {
                tbSppName4.Text = paramList[0];
                tbLongevity4.Text = paramList[1];
                tbShadeTol4.Text = paramList[2];
                tbAgeMort4.Text = paramList[3];
                tbLeaf4.Text = paramList[4];
                tbDecay4.Text = paramList[5];
                tbVegProb4.Text = paramList[6];
                tbMinVegAge4.Text = paramList[7];
                tbMaxVegAge4.Text = paramList[8];
                tbANPPmax4.Text = paramList[9];
                tbBioMax4.Text = paramList[10];
                tbMortShape4.Text = paramList[11];
                tbPower4.Text = paramList[12];
                tbMortMod4.Text = paramList[13];
                tbAgeMort4.Text = paramList[14];
                tbSEC4.Text = paramList[15];
            }
            else if (num5 == 5)
            {
                tbSppName5.Text = paramList[0];
                tbLongevity5.Text = paramList[1];
                tbShadeTol5.Text = paramList[2];
                tbAgeMort5.Text = paramList[3];
                tbLeaf5.Text = paramList[4];
                tbDecay5.Text = paramList[5];
                tbVegProb5.Text = paramList[6];
                tbMinVegAge5.Text = paramList[7];
                tbMaxVegAge5.Text = paramList[8];
                tbANPPmax5.Text = paramList[9];
                tbBioMax5.Text = paramList[10];
                tbMortShape5.Text = paramList[11];
                tbPower5.Text = paramList[12];
                tbMortMod5.Text = paramList[13];
                tbAgeMort5.Text = paramList[14];
                tbSEC5.Text = paramList[15];
            }
            else if (num6 == 5)
            {
                tbSppName6.Text = paramList[0];
                tbLongevity6.Text = paramList[1];
                tbShadeTol6.Text = paramList[2];
                tbAgeMort6.Text = paramList[3];
                tbLeaf6.Text = paramList[4];
                tbDecay6.Text = paramList[5];
                tbVegProb6.Text = paramList[6];
                tbMinVegAge6.Text = paramList[7];
                tbMaxVegAge6.Text = paramList[8];
                tbANPPmax6.Text = paramList[9];
                tbBioMax6.Text = paramList[10];
                tbMortShape6.Text = paramList[11];
                tbPower6.Text = paramList[12];
                tbMortMod6.Text = paramList[13];
                tbAgeMort6.Text = paramList[14];
                tbSEC6.Text = paramList[15];
            }

        }

        private void load6ParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);


            } while (!fileReader.EndOfStream);

            int num1 = int.Parse(tbSppNum1.Text);
            int num2 = int.Parse(tbSppNum2.Text);
            int num3 = int.Parse(tbSppNum3.Text);
            int num4 = int.Parse(tbSppNum4.Text);
            int num5 = int.Parse(tbSppNum5.Text);
            int num6 = int.Parse(tbSppNum6.Text);

            if (num1 == 6)
            {
                tbSppName.Text = paramList[0];
                tbLongevity.Text = paramList[1];
                tbShadeTol.Text = paramList[2];
                tbAgeMort1.Text = paramList[3];
                tbLeaf.Text = paramList[4];
                tbDecay1.Text = paramList[5];
                tbVegProb1.Text = paramList[6];
                tbMinVegAge1.Text = paramList[7];
                tbMaxVegAge1.Text = paramList[8];
                tbANPPmax.Text = paramList[9];
                tbBioMax.Text = paramList[10];
                tbMortShape1.Text = paramList[11];
                tbPower1.Text = paramList[12];
                tbMortMod1.Text = paramList[13];
                tbAgeMort1.Text = paramList[14];
                tbSEC.Text = paramList[15];
            }
            else if (num2 == 6)
            {
                tbSppName2.Text = paramList[0];
                tbLongevity2.Text = paramList[1];
                tbShadeTol2.Text = paramList[2];
                tbAgeMort2.Text = paramList[3];
                tbLeaf2.Text = paramList[4];
                tbDecay2.Text = paramList[5];
                tbVegProb2.Text = paramList[6];
                tbMinVegAge2.Text = paramList[7];
                tbMaxVegAge2.Text = paramList[8];
                tbANPPmax2.Text = paramList[9];
                tbBioMax2.Text = paramList[10];
                tbMortShape2.Text = paramList[11];
                tbPower2.Text = paramList[12];
                tbMortMod2.Text = paramList[13];
                tbAgeMort2.Text = paramList[14];
                tbSEC2.Text = paramList[15];
            }
            else if (num3 == 6)
            {
                tbSppName3.Text = paramList[0];
                tbLongevity3.Text = paramList[1];
                tbShadeTol3.Text = paramList[2];
                tbAgeMort3.Text = paramList[3];
                tbLeaf3.Text = paramList[4];
                tbDecay3.Text = paramList[5];
                tbVegProb3.Text = paramList[6];
                tbMinVegAge3.Text = paramList[7];
                tbMaxVegAge3.Text = paramList[8];
                tbANPPmax3.Text = paramList[9];
                tbBioMax3.Text = paramList[10];
                tbMortShape3.Text = paramList[11];
                tbPower3.Text = paramList[12];
                tbMortMod3.Text = paramList[13];
                tbAgeMort3.Text = paramList[14];
                tbSEC3.Text = paramList[15];
            }
            else if (num4 == 6)
            {
                tbSppName4.Text = paramList[0];
                tbLongevity4.Text = paramList[1];
                tbShadeTol4.Text = paramList[2];
                tbAgeMort4.Text = paramList[3];
                tbLeaf4.Text = paramList[4];
                tbDecay4.Text = paramList[5];
                tbVegProb4.Text = paramList[6];
                tbMinVegAge4.Text = paramList[7];
                tbMaxVegAge4.Text = paramList[8];
                tbANPPmax4.Text = paramList[9];
                tbBioMax4.Text = paramList[10];
                tbMortShape4.Text = paramList[11];
                tbPower4.Text = paramList[12];
                tbMortMod4.Text = paramList[13];
                tbAgeMort4.Text = paramList[14];
                tbSEC4.Text = paramList[15];
            }
            else if (num5 == 6)
            {
                tbSppName5.Text = paramList[0];
                tbLongevity5.Text = paramList[1];
                tbShadeTol5.Text = paramList[2];
                tbAgeMort5.Text = paramList[3];
                tbLeaf5.Text = paramList[4];
                tbDecay5.Text = paramList[5];
                tbVegProb5.Text = paramList[6];
                tbMinVegAge5.Text = paramList[7];
                tbMaxVegAge5.Text = paramList[8];
                tbANPPmax5.Text = paramList[9];
                tbBioMax5.Text = paramList[10];
                tbMortShape5.Text = paramList[11];
                tbPower5.Text = paramList[12];
                tbMortMod5.Text = paramList[13];
                tbAgeMort5.Text = paramList[14];
                tbSEC5.Text = paramList[15];
            }
            else if (num6 == 6)
            {
                tbSppName6.Text = paramList[0];
                tbLongevity6.Text = paramList[1];
                tbShadeTol6.Text = paramList[2];
                tbAgeMort6.Text = paramList[3];
                tbLeaf6.Text = paramList[4];
                tbDecay6.Text = paramList[5];
                tbVegProb6.Text = paramList[6];
                tbMinVegAge6.Text = paramList[7];
                tbMaxVegAge6.Text = paramList[8];
                tbANPPmax6.Text = paramList[9];
                tbBioMax6.Text = paramList[10];
                tbMortShape6.Text = paramList[11];
                tbPower6.Text = paramList[12];
                tbMortMod6.Text = paramList[13];
                tbAgeMort6.Text = paramList[14];
                tbSEC6.Text = paramList[15];
            }

        }

        private void cbOutputFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOutputFolder.Checked)
                tbOutputFolder.Enabled = true;
            else
                tbOutputFolder.Enabled = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //save graphs
            GraphPane myPane1 = graph1.GraphPane;
            string graphFN1 = baseFileName+"_Biomass" + (repCount - 1).ToString() + ".bmp";
            string outGraph1 = Path.Combine(dirName, graphFN1);
            myPane1.GetImage().Save(outGraph1);

            GraphPane myPane2 = graph2.GraphPane;
            string graphFN2 = baseFileName + "_PercentShade" + (repCount - 1).ToString() + ".bmp";
            string outGraph2 = Path.Combine(dirName, graphFN2);
            myPane2.GetImage().Save(outGraph2);

            GraphPane myPane3 = graph3.GraphPane;
            string graphFN3 = baseFileName + "_ShadeClass" + (repCount - 1).ToString() + ".bmp";
            string outGraph3 = Path.Combine(dirName, graphFN3);
            myPane3.GetImage().Save(outGraph3);

            GraphPane myPane4 = graph4.GraphPane;
            string graphFN4 = baseFileName + "_NumCohorts" + (repCount - 1).ToString() + ".bmp";
            string outGraph4 = Path.Combine(dirName, graphFN4);
            myPane4.GetImage().Save(outGraph4);

            GraphPane myPane5 = graph_BudwormD.GraphPane;
            string graphFN5 = baseFileName + "_BudwormDensity" + (repCount - 1).ToString() + ".bmp";
            string outGraph5 = Path.Combine(dirName, graphFN5);
            myPane5.GetImage().Save(outGraph5);

            GraphPane myPane6 = graph6.GraphPane;
            string graphFN6 = baseFileName + "_CohortANPP" + (repCount - 1).ToString() + ".bmp";
            string outGraph6 = Path.Combine(dirName, graphFN6);
            myPane6.GetImage().Save(outGraph6);

            GraphPane myPane7 = graph_BudwormC.GraphPane;
            string graphFN7 = baseFileName + "_BudwormCount" + (repCount - 1).ToString() + ".bmp";
            string outGraph7 = Path.Combine(dirName, graphFN7);
            myPane7.GetImage().Save(outGraph7);

            GraphPane myPane8 = graph_EnemyDens.GraphPane;
            string graphFN8 = baseFileName + "_EnemyDensity" + (repCount - 1).ToString() + ".bmp";
            string outGraph8 = Path.Combine(dirName, graphFN8);
            myPane8.GetImage().Save(outGraph8);

            GraphPane myPane9 = graph9.GraphPane;
            string graphFN9 = baseFileName + "_CohortBiomass" + (repCount - 1).ToString() + ".bmp";
            string outGraph9 = Path.Combine(dirName, graphFN9);
            myPane9.GetImage().Save(outGraph9);

            GraphPane myPane10 = graph10.GraphPane;
            string graphFN10 = baseFileName + "_DeadWoodyBio" + (repCount - 1).ToString() + ".bmp";
            string outGraph10 = Path.Combine(dirName, graphFN10);
            myPane10.GetImage().Save(outGraph10);

            GraphPane myPane11 = graph_EnemyCount.GraphPane;
            string graphFN11 = baseFileName + "_EnemyCount" + (repCount - 1).ToString() + ".bmp";
            string outGraph11 = Path.Combine(dirName, graphFN11);
            myPane11.GetImage().Save(outGraph11);

            GraphPane myPane12 = graph_HostFoliage.GraphPane;
            string graphFN12 = baseFileName + "_HostFoliage" + (repCount - 1).ToString() + ".bmp";
            string outGraph12 = Path.Combine(dirName, graphFN12);
            myPane12.GetImage().Save(outGraph12);

            GraphPane myPane13 = graph_PropDefol.GraphPane;
            string graphFN13 = baseFileName + "_PropDefol" + (repCount - 1).ToString() + ".bmp";
            string outGraph13 = Path.Combine(dirName, graphFN13);
            myPane13.GetImage().Save(outGraph13);

            GraphPane myPane14 = graph_LogPopDens.GraphPane;
            string graphFN14 = baseFileName + "_LogPopDens" + (repCount - 1).ToString() + ".bmp";
            string outGraph14 = Path.Combine(dirName, graphFN14);
            myPane14.GetImage().Save(outGraph14);
        }

        private void budwormToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            //Budworm - Declare input parameters
            string budwormStartYear = "0";
            string budwormrm = "0";
            string budwormrprimem = "0";
            string budwormb = "0";
            string budwormbprime = "0";
            string budwormc = "0";
            string budwormcprime = "0";
            string budwormaprime = "0";
            string budEnemyDensity0 = "0";
            string budwormDensity0 = "0";
            string budwormMaxDensity = "0";
            string budwormWinterMean = "1";
            string budwormWinterStdev = "0";
            string budwormPredationMean = "1";
            string budwormPredationStdev = "0";
            string matingA = "0";
            string matingB = "0";
            string matingC = "1";
            string decidProtectDmax1 = "0";
            string decidProtectDmax2 = "0";
            string phenolLimitMean = "1";
            string phenolLimitStdev = "0";
            string defolFecundityCheck = "1";
            string growthReductionCheck = "1";
            string mortalityCheck = "1";
            string defolLambda = "0.255";
            string emigrationCheck = "1";
            string minLDD = "0";
            string halfLDD = "0";
            string maxLDD = "0";
            string maxLDDProp = "0";
            string positiveFecundCheck = "1";
            string minSuscAge = "0";
            string ageMortCheck = "1";
            string predM = "1";
            string predN = "1";
            string preyM = "1";
            string preyN = "1";
            //Budworm - END - Declare input parameters

            //Budworm - Read parameter values from text boxes
            budwormStartYear = (tbBudStartYear.Text);
            budwormrm = (tbBudrm.Text);
            budwormrprimem = (tbBudrprimem.Text);
            budwormb = (tbBudb.Text);
            budwormbprime = (tbBudbprime.Text);
            budwormc = (tbBudc.Text);
            budwormcprime = (tbBudcprime.Text);
            budwormaprime = (tbBudaprime.Text);
            budEnemyDensity0 = (tbBudx0.Text);
            budwormDensity0 =(tbBudy0.Text);
            budwormMaxDensity = (tbBudMaxDensity.Text);
            budwormWinterMean = (tbBudWinterMean.Text);
            budwormWinterStdev = (tbBudWinterStdev.Text);
            budwormPredationMean = (tbBudPredationMean.Text);
            budwormPredationStdev =(tbBudPredationStdev.Text);
            matingA = (tbAlleeShape.Text);
            matingB = (tbAlleeScale.Text);
            matingC = (tbMatingC.Text);
            decidProtectDmax1 = (tbDecidProtectD1.Text);
            decidProtectDmax2 = (tbDecidProtectD2.Text);
            phenolLimitMean = (tbBudPhenolMean.Text);
            phenolLimitStdev = (tbBudPhenolStdev.Text);
            if (cbDefolFecund.Checked) defolFecundityCheck = "1"; else defolFecundityCheck = "0";
            if (cbBudGR.Checked) growthReductionCheck = "1"; else growthReductionCheck = "0";
            if (cbBudmort.Checked) mortalityCheck = "1"; else mortalityCheck = "0";
            defolLambda = (tbDefolLambda.Text);
            if (cbEmigration.Checked) emigrationCheck = "1"; else emigrationCheck = "0";
            minLDD = (tbMinLDD.Text);
            halfLDD = (tbHalfLDD.Text);
            maxLDD = (tbMaxLDD.Text);
            maxLDDProp = (tbMaxLDDProp.Text);
            if (cbPositiveFecund.Checked) positiveFecundCheck = "1"; else positiveFecundCheck = "0";
            minSuscAge =(tbMinSuscAge.Text);
            if (cbAgeMort.Checked) ageMortCheck = "1"; else ageMortCheck = "0";
            predM = (tbPredM.Text);
            predN = (tbPredN.Text);
            preyM = (tbPreyM.Text);
            preyN = (tbPreyN.Text);

            List<string> paramList = new List<string>();
            paramList.Add(budwormStartYear);
            paramList.Add(budwormrm);
            paramList.Add(budwormrprimem);
            paramList.Add(budwormb);
            paramList.Add(budwormbprime);
            paramList.Add(budwormc);
            paramList.Add(budwormcprime);
            paramList.Add(budwormaprime);
            paramList.Add(budEnemyDensity0);
            paramList.Add(budwormDensity0);
            paramList.Add(budwormMaxDensity);
            paramList.Add(budwormWinterMean);
            paramList.Add(budwormWinterStdev);
            paramList.Add(budwormPredationMean);
            paramList.Add(budwormPredationStdev);
            paramList.Add(matingA);
            paramList.Add(matingB);
            paramList.Add(matingC);
            paramList.Add(decidProtectDmax1);
            paramList.Add(decidProtectDmax2);
            paramList.Add(phenolLimitMean);
            paramList.Add(phenolLimitStdev);
            paramList.Add(defolFecundityCheck);
            paramList.Add(growthReductionCheck);
            paramList.Add(mortalityCheck);
            paramList.Add(defolLambda);
            paramList.Add(emigrationCheck);
            paramList.Add(minLDD);
            paramList.Add(halfLDD);
            paramList.Add(maxLDD);
            paramList.Add(maxLDDProp);
            paramList.Add(positiveFecundCheck);
            paramList.Add(minSuscAge);
            paramList.Add(ageMortCheck);
            paramList.Add(predM);
            paramList.Add(predN);
            paramList.Add(preyM);
            paramList.Add(preyN);
            
            bool runSave = false;
            Form4 saveForm = new Form4();
            saveForm.ShowDialog();
            runSave = saveForm.cbSave.Checked;

            if (runSave)
            {
                string outTextFile = saveForm.tbFileName.Text;
                StringBuilder sb = new StringBuilder();

                foreach (string s in paramList)
                {
                    sb.AppendLine(s);
                }
                File.WriteAllText(outTextFile, sb.ToString());
            }

        }

        private void budwormToolStripMenuItem_Click(object sender, EventArgs e)
        {

            bool runLoad = false;
            Form5 loadForm = new Form5();
            loadForm.ShowDialog();
            runLoad = loadForm.cbLoad.Checked;

            string fileName = loadForm.tbFileName.Text;
            StreamReader fileReader;
            fileReader = new StreamReader(fileName);
            string line;

            List<string> paramList = new List<string>();

            do
            {
                line = fileReader.ReadLine();

                string[] inputList = Regex.Split(line, "\\s+");
                paramList.Add(inputList[0]);


            } while (!fileReader.EndOfStream);

            tbBudStartYear.Text = paramList[0];
            tbBudrm.Text = paramList[1];
            tbBudrprimem.Text = paramList[2];
            tbBudb.Text = paramList[3];
            tbBudbprime.Text = paramList[4];
            tbBudc.Text = paramList[5];
            tbBudcprime.Text = paramList[6];
            tbBudaprime.Text = paramList[7];
            tbBudx0.Text = paramList[8];
            tbBudy0.Text = paramList[9];
            tbBudMaxDensity.Text = paramList[10];
            tbBudWinterMean.Text = paramList[11];
            tbBudWinterStdev.Text = paramList[12];
            tbBudPredationMean.Text = paramList[13];
            tbBudPredationStdev.Text = paramList[14];
            tbAlleeShape.Text = paramList[15];
            tbAlleeScale.Text = paramList[16];
            tbMatingC.Text = paramList[17];
            tbDecidProtectD1.Text = paramList[18];
            tbDecidProtectD2.Text = paramList[19];
            tbBudPhenolMean.Text = paramList[20];
            tbBudPhenolStdev.Text = paramList[21];
            if (paramList[22] == "1") cbDefolFecund.Checked = true; else cbDefolFecund.Checked = false;
            if (paramList[23] == "1") cbBudGR.Checked = true; else cbBudGR.Checked = false;
            if (paramList[24] == "1") cbBudmort.Checked = true; else cbBudmort.Checked = false;
            tbDefolLambda.Text = paramList[25];
            if (paramList[26] == "1") cbEmigration.Checked = true; else cbEmigration.Checked = false;
            tbMinLDD.Text = paramList[27];
            tbHalfLDD.Text = paramList[28];
            tbMaxLDD.Text = paramList[29];
            tbMaxLDDProp.Text = paramList[30];
            if (paramList[31] == "1") cbPositiveFecund.Checked = true; else cbPositiveFecund.Checked = false;
            tbMinSuscAge.Text = paramList[32];
            if (paramList[33] == "1") cbAgeMort.Checked = true; else cbAgeMort.Checked = false;
            tbPredM.Text = paramList[34];
            tbPredN.Text = paramList[35];
            tbPreyM.Text = paramList[36];
            tbPreyN.Text = paramList[37];

        }

        private void cbRandSeed_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRandSeed.Checked)
            {
                label99.Enabled = true;
                tbRandSeed.Enabled = true;
            }
            else
            {
                label99.Enabled = false;
                tbRandSeed.Enabled = false;
            }
        }







    }
}