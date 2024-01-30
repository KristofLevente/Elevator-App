using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElevatorApp
{
    class Elevator
    {
        private int location;
        private List<Button> floorNumberButtons;

        public Elevator(int location, List<Button> floorNumberButtons)
        {
            this.location = location;
            this.floorNumberButtons = floorNumberButtons;
        }

        public void setLocation(int location)
        {
            this.location = location;
        }

        public int getLocation()
        {
            return location;
        }

        public List<Button> getButtonList()
        {
            return floorNumberButtons;
        }

        public void addButtonToList(Button button)
        {
            floorNumberButtons.Add(button);
        }

        public void SendElevatorToFloor(Button button)
        {
            int buttonValue = int.Parse(button.Text);
            if(buttonValue != this.location)
            {
                setLocation(buttonValue);
            }
        }
        public void SendElevatorToFloor(int floor)
        {
                setLocation(floor);
        }

    }
}
