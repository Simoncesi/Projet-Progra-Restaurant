using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Controller
{
    public class Loader
    {
        private int IDCount;
        private Core core;
        private World world;

        public Loader(Core core, World world)
        {
            IDCount = 0;
            this.core = core;
            this.world = world;
        }

        public int GiveID()
        {
            return IDCount++;
        }

        public Core GetCore()
        {
            return core;
        }

        public World GetWorld()
        {
            return world;
        }
    }

    public class Core
    {
        private List<Personnage> personnages;
        private System.Timers.Timer aTimer;
        private int timeElapsed;

        public Core()
        {
            personnages = new List<Personnage>();
            SetTimeElapsed(60);

            SetTimer();
        }

        public void AddPersonnage(Personnage personnage)
        {
            personnages.Add(personnage);
        }

        public void DelPersonnage(Personnage personnage)
        {
            personnages.Remove(personnage);
        }

        public void SetTimeElapsed(int timeElapsed)
        {
            this.timeElapsed = timeElapsed;
        }

        public void Pause(bool pause)
        {
            if(pause==true)
            {
                aTimer.Enabled = false;
            }
            else
            {
                aTimer.Enabled = true;
            }
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach(Personnage personnage in personnages.ToList())
            {
                personnage.UpdatePersonnage(timeElapsed);
            }
        }
    }

}
