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

        public void Simulation_tic()
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

        public List<double> Simulate_time(double time=0, Isotope.Radionuclide nuc = Isotope.Radionuclide.Rn_222)
        {
            List<double> data = new List<double>();
            double current_time = 0;
            double measured_activity = 0;
            while(current_time < time)
            {
                Simulation_tic();
                foreach(Isotope isotope in atoms)
                {
                    if(isotope.Get_Radionuclide() == nuc)
                    {
                        measured_activity = isotope.Get_activity();
                    }
                }
                data.Add(measured_activity);
                current_time+=time_tic;
            }
            return data;
        }

        public double Get_timetic()
        {
            return time_tic;
        }
            
        
    }
}
