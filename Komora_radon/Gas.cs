using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komora_radon
{
    class Gas
    {
        List<Isotope> atoms = new List<Isotope>();
        double time_tic;
        double time;
        int isotope_num;


        public Gas(double tic, double radium_act=0)
        {
            time_tic = tic;
            time = 0;

            Radium_init(radium_act);
        }

        private void Radium_init(double act = 0)
        {
            atoms.Add(new Isotope(Isotope.Radionuclide.Ra_226, act, Isotope.Radionuclide.Rn_222)); //Decay time!!
            atoms.Add(new Isotope(Isotope.Radionuclide.Rn_222));
            atoms.Add(new Isotope(Isotope.Radionuclide.Po_218));
            atoms.Add(new Isotope(Isotope.Radionuclide.At_218));
            atoms.Add(new Isotope(Isotope.Radionuclide.Rn_218));
            atoms.Add(new Isotope(Isotope.Radionuclide.Pb_214));
            atoms.Add(new Isotope(Isotope.Radionuclide.Bi_214));
            atoms.Add(new Isotope(Isotope.Radionuclide.Po_214));
            atoms.Add(new Isotope(Isotope.Radionuclide.Tl_210));
            atoms.Add(new Isotope(Isotope.Radionuclide.Pb_210));
            atoms.Add(new Isotope(Isotope.Radionuclide.Un)); //Last stable isotope

            isotope_num = atoms.Count;
        }

        public void Simulation_tic(double time=1.0)
        {
            double change = 0;
            Isotope.Radionuclide prev = Isotope.Radionuclide.Ra_226;
            
            /*
            for(int i=0; i < isotope_num-1; i++)
            {
                //((Isotope)atoms[i]).Decay(atoms[i + 1], time_tic); // Do it!


            }
            */

            foreach(Isotope isotope in atoms)
            {
                //int next_index = atoms.IndexOf(isotope) + 1;
                if (isotope.Get_progeny() != prev)
                {
                    change = 0;  
                }
                if(isotope.Get_activity() != 0)
                    change = isotope.Decay(time_tic, change);
                prev = isotope.Get_progeny();
            }
        }
            
        
    }
}
