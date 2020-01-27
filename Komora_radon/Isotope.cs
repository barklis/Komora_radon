using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komora_radon
{
    class Isotope
    {
        string isotope_name;

        public double ln2; 
        public enum Emission_type {BETA, ALPHA};
        public enum Radionuclide {Ra_226, Rn_222, Po_218, At_218, Rn_218, Pb_214, Bi_214, Po_214, Tl_210, Pb_210, Un};
        Radionuclide nuclide;
        Emission_type decay_type;
        int Z;
        int A;
        double half_life;
        double activity;
        double N;

        //Advanced decay:
        Radionuclide progeny;
        double A_prob;
        double B_prob;

        //For dose rate sim:
        bool excited;
        double gamma_average_energy;
        double bio_factor; 


        public Isotope(string name, double half_l, int z = 0, int a = 0, double activ=0, Emission_type type=Emission_type.ALPHA, double a_prob=1.0, Radionuclide progen=Radionuclide.Un)
        {
            ln2 = Math.Log(2);
            
            nuclide = Radionuclide.Un;
            isotope_name = name;

            half_life = half_l;
            activity = activ;
            N = activity * half_life / ln2; //Check this!
            decay_type = type;
            Z = z;
            A = a;

            //Advanced:
            progeny = progen;
            A_prob = a_prob;
            B_prob = 1 - A_prob;

            //Dose sim:
            excited = false;

        }

        public Isotope(Radionuclide nuc, double activ = 0, Radionuclide prog=Radionuclide.Un)
        {
            ln2 = Math.Log(2);
            nuclide = nuc;
            isotope_name = Isotope_by_name(nuc);
            A_Z_Radionuclide(nuc, out A, out Z);
            decay_type = Emission_Radionuclide(nuc);
            half_life = halflife_Radionuclide(nuc);
            activity = activ;
            N = activity * half_life / ln2;
            progeny = prog;
        }


        private string Isotope_by_name(Radionuclide name)
        {
            string isotope_name = "Unknown";
            switch(name)
            {
                case Radionuclide.Ra_226:
                    isotope_name = "Radium-226";
                    break;
                case Radionuclide.Rn_222:
                    isotope_name = "Radon-222";
                    break;
                case Radionuclide.Po_218:
                    isotope_name = "Polunium-218";
                    break;
                case Radionuclide.At_218:
                    isotope_name = "Astatium-222";
                    break;
                case Radionuclide.Rn_218:
                    isotope_name = "Radon-218";
                    break;
                case Radionuclide.Pb_214:
                    isotope_name = "Plumbum-214";
                    break;
                case Radionuclide.Bi_214:
                    isotope_name = "Bismutum-214";
                    break;
                case Radionuclide.Po_214:
                    isotope_name = "Polonium-214";
                    break;
                case Radionuclide.Tl_210:
                    isotope_name = "Thallium-210";
                    break;
                case Radionuclide.Pb_210:
                    isotope_name = "Plumbum-210";
                    break;
                default:
                    break;
            }
            return isotope_name;
        }

        public Radionuclide Isotope_by_A_Z(int a, int z)
        {
            Radionuclide nuc = Radionuclide.Un;
            switch (z)
            {
                case 88:
                    if (226 == a)
                        nuc = Radionuclide.Ra_226;
                    else
                        nuc = Radionuclide.Un;
                    break;
                case 86:
                    if (222 == a)
                        nuc = Radionuclide.Rn_222;
                    else if (218 == a)
                        nuc = Radionuclide.Rn_218;
                    else
                        nuc = Radionuclide.Un;
                    break;
                case 85:
                    if (218 == a)
                        nuc = Radionuclide.At_218;
                    else
                        nuc = Radionuclide.Un;
                    break;
                case 84:
                    if (218 == a)
                        nuc = Radionuclide.Po_218;
                    else if (214 == a)
                        nuc = Radionuclide.Po_214;
                    else
                        nuc = Radionuclide.Un;
                    break;
                case 82:
                    if (214 == a)
                        nuc = Radionuclide.Pb_214;
                    else if (210 == a)
                        nuc = Radionuclide.Pb_210;
                    else
                        nuc = Radionuclide.Un;
                    break;
                case 81:
                    if (210 == a)
                        nuc = Radionuclide.Tl_210;
                    else
                        nuc = Radionuclide.Un;
                    break;
                default:
                    break;
            }
            return nuc;
        }

        public void A_Z_Radionuclide(Radionuclide nuc, out int A, out int Z)
        {
            
            switch (nuc)
            {
                case Radionuclide.Ra_226:
                    A = 226;
                    Z = 88;
                    break;
                case Radionuclide.Rn_222:
                    A = 222;
                    Z = 86;
                    break;
                case Radionuclide.Po_218:
                    A = 218;
                    Z = 84;
                    break;
                case Radionuclide.At_218:
                    A = 218;
                    Z = 85;
                    break;
                case Radionuclide.Rn_218:
                    A = 218;
                    Z = 86;
                    break;
                case Radionuclide.Pb_214:
                    A = 214;
                    Z = 82;
                    break;
                case Radionuclide.Bi_214:
                    A = 214;
                    Z = 83;
                    break;
                case Radionuclide.Po_214:
                    A = 214;
                    Z = 84;
                    break;
                case Radionuclide.Tl_210:
                    A = 210;
                    Z = 81;
                    break;
                case Radionuclide.Pb_210:
                    A = 210;
                    Z = 82;
                    break;
                default:
                    A = 0;
                    Z = 0;
                    break;
            }
        }

        public Emission_type Emission_Radionuclide(Radionuclide nuc)
        {
            Emission_type em;

            //FILL THIS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            switch (nuc)
            {
                case Radionuclide.Ra_226:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Rn_222:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Po_218:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.At_218:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Rn_218:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Pb_214:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Bi_214:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Po_214:
                    em = Emission_type.ALPHA;
                    break;
                case Radionuclide.Tl_210:
                    em = Emission_type.BETA;
                    break;
                case Radionuclide.Pb_210:
                    em = Emission_type.ALPHA;
                    break;
                default:
                    em = Emission_type.ALPHA;
                    break;
            }
            return em;
        }

        public double halflife_Radionuclide(Radionuclide nuc)
        {
            double hl;

            //FILL THIS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            switch (nuc)
            {
                case Radionuclide.Ra_226:
                    hl = 1600;
                    break;
                case Radionuclide.Rn_222:
                    hl = 0;
                    break;
                case Radionuclide.Po_218:
                    hl = 0;
                    break;
                case Radionuclide.At_218:
                    hl = 0;
                    break;
                case Radionuclide.Rn_218:
                    hl = 0;
                    break;
                case Radionuclide.Pb_214:
                    hl = 0;
                    break;
                case Radionuclide.Bi_214:
                    hl = 0;
                    break;
                case Radionuclide.Po_214:
                    hl = 0;
                    break;
                case Radionuclide.Tl_210:
                    hl = 0;
                    break;
                case Radionuclide.Pb_210:
                    hl = 0;
                    break;
                default:
                    hl = 0;
                    break;
            }
            return hl;
        }

        public double Decay(double time_tic, double prev=0)
        {
            activity += prev;
            double change = activity * Math.Exp(-1 * time_tic * ln2 / half_life);
            activity -= change;
            N = activity * half_life / ln2;

            //Isotope product = new Isotope(Isotope.Radionuclide.Po_214, 0); //Error here !
            //product.activity += change;
            //product.N = product.activity * half_life / ln2;

            return change;
        }

        public double Emmision(double volume)
        {
            return bio_factor * gamma_average_energy * N / volume;
        }

        public Radionuclide Get_progeny()
        {
            return progeny;
        }

        public double Get_activity()
        {
            return activity;
        }

    }
}
