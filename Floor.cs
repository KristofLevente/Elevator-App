using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElevatorApp
{
    class Floor
    {
        private int location;
        private Button callButton;

        public Floor(int location, Button callButton)
        {
            this.location = location;
            this.callButton = callButton;
        }
        public int getLocation()
        {
            return location;
        }

        public Button getButton()
        {
            return callButton;
        }

        
    }
}

