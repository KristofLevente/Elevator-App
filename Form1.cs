using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElevatorApp
{
    public partial class Form1 : Form
    {
        

        public static List<Button> ElevatorButtonsA = new List<Button>(); 
        public static List<Button> ElevatorButtonsB = new List<Button>();
        List<Floor> Floors = new List<Floor>();
        Elevator elevatorA = new Elevator(0, ElevatorButtonsA);
        Elevator elevatorB = new Elevator(6, ElevatorButtonsB);
        Label elevatorCounterA = new Label();
        Label elevatorCounterB = new Label();
        Label statusLabel = new Label();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timer3 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timer4 = new System.Windows.Forms.Timer();
        int timercounter;

        public Form1()
        {
            InitializeComponent();

            InitializeComponents();
        }

        private void InitializeComponents()
        {

            Label elevator1 = new Label();
            elevator1.Text = "Elevator A";
            elevator1.Size = new Size(85, 50);
            elevator1.Font = new Font("Arial", 10, FontStyle.Bold);
            elevator1.Location = new Point(100, 100);
            Controls.Add(elevator1);

            Label elevator2 = new Label();
            elevator2.Text = "Elevator B";
            elevator2.Size = new Size(85, 50);
            elevator2.Font = new Font("Arial", 10, FontStyle.Bold);
            elevator2.Location = new Point(ClientSize.Width - 250, 100);
            Controls.Add(elevator2);

            Label floors = new Label();
            floors.Text = "Call buttons for floors";
            floors.Size = new Size(200, 50);
            floors.Font = new Font("Arial", 10, FontStyle.Bold);
            floors.Location = new Point(ClientSize.Width/2 -100, 100);
            Controls.Add(floors);

            elevatorCounterA.Text = elevatorA.getLocation().ToString();
            elevatorCounterA.Size = new Size(50, 50);
            elevatorCounterA.Font = new Font("Arial", 30, FontStyle.Bold);
            elevatorCounterA.Location = new Point(116, 150);
            elevatorCounterA.ForeColor = Color.Red;
            Controls.Add(elevatorCounterA);

            int x = 0;
            for(int i=0; i<7; i++)
            {
                Button elevatorButtonA = new Button();
                elevatorButtonA.Text = $"{i}";
                elevatorButtonA.Name = $"ElevatorA{i}";
                elevatorButtonA.Location = new Point(100, x + 200);
                elevatorButtonA.Click += ElevatorSendButton_Clicked;

                elevatorA.addButtonToList(elevatorButtonA);

                Controls.Add(elevatorButtonA);
                x = x + 30;
            }

            
            elevatorCounterB.Text = elevatorB.getLocation().ToString();
            elevatorCounterB.Size = new Size(50, 50);
            elevatorCounterB.Font = new Font("Arial", 30, FontStyle.Bold);
            elevatorCounterB.Location = new Point(ClientSize.Width - 234, 150);
            elevatorCounterB.ForeColor = Color.Red;
            Controls.Add(elevatorCounterB);

            statusLabel.Text = "";
            statusLabel.Size = new Size(300, 50);
            statusLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            statusLabel.Location = new Point(ClientSize.Width /2 -100, 0);

            Controls.Add(statusLabel);

            int y = 0;
            for (int i = 0; i < 7; i++)
            {
                Button elevatorButtonB = new Button();
                elevatorButtonB.Text = $"{i}";
                elevatorButtonB.Name = $"ElevatorB{i}";
                elevatorButtonB.Location = new Point(ClientSize.Width -250, y + 200);
                elevatorButtonB.Click += ElevatorSendButton_Clicked;

                elevatorB.addButtonToList(elevatorButtonB);

                Controls.Add(elevatorButtonB);
                y = y + 30;
            }

            int z = 0;
            for(int i=0; i<7; i++)
            {
                Button callButton = new Button();
                callButton.Text = $"Call to floor{i}";
                callButton.Name = $"FloorButton{i}";
                callButton.Location = new Point(ClientSize.Width /2 - 60, z + 200);
                callButton.Click += CallButton_Click;

                Floor floor = new Floor(i, callButton);
                Floors.Add(floor);

                Controls.Add(callButton);
                z = z + 30;
            }


        }

        private void CallButton_Click(object sender, EventArgs e)
        {
            int floorNumber=0;
            Button button = (Button)sender;

            for(int i=0; i<Floors.Count; ++i)
            {
                if(Floors[i].getButton() == button)
                {
                    floorNumber = Floors[i].getLocation();
                    int distanceA = Math.Abs(elevatorA.getLocation() - floorNumber);
                    int distanceB = Math.Abs(elevatorB.getLocation() - floorNumber);

                    if (distanceA < distanceB || (distanceA == distanceB && elevatorA.getLocation() < elevatorB.getLocation()))
                    {
                        int oldLocation = elevatorA.getLocation();
                        elevatorA.SendElevatorToFloor(floorNumber);

                        if (elevatorA.getLocation() > oldLocation)
                        {
                            statusLabel.Text = "Elevator A is going Up";
                            timercounter = oldLocation;
                            timer.Interval = 1500;
                            timer.Tick += Timer_Tick_A;
                            timer.Start();

                        }
                        else if (elevatorA.getLocation() < oldLocation)
                        {
                            statusLabel.Text = "Elevator A is going Down";
                            timercounter = oldLocation;
                            timer2.Interval = 1500;
                            timer2.Tick += Timer_Tick_Back_A;
                            timer2.Start();
                        }

                    }
                    else
                    {
                        int oldLocationB = elevatorB.getLocation();
                        elevatorB.SendElevatorToFloor(floorNumber);

                        if (elevatorB.getLocation() > oldLocationB)
                        {
                            statusLabel.Text = "Elevator B is going Up";
                            timercounter = oldLocationB;
                            timer3.Interval = 1500;
                            timer3.Tick += Timer_Tick_B;
                            timer3.Start();

                        }
                        else if (elevatorB.getLocation() < oldLocationB)
                        {
                            statusLabel.Text = "Elevator B is going Down";
                            timercounter = oldLocationB;
                            timer4.Interval = 1500;
                            timer4.Tick += Timer_Tick_Back_B;
                            timer4.Start();
                        }
                    }

                }
            }
            
        }

        private void ElevatorSendButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            timercounter = 0;
            if (elevatorA.getButtonList().Contains(button))
            {
                int oldLocation = elevatorA.getLocation();
                
                elevatorA.SendElevatorToFloor(button);
                
                if(elevatorA.getLocation() > oldLocation)
                {
                    statusLabel.Text = "Elevator A is going Up";
                    timercounter = oldLocation;
                    timer.Interval = 1500;
                    timer.Tick += Timer_Tick_A;
                    timer.Start();

                }
                else if(elevatorA.getLocation() < oldLocation)
                {
                    statusLabel.Text = "Elevator A is going Down";
                    timercounter = oldLocation;
                    timer2.Interval = 1500;
                    timer2.Tick += Timer_Tick_Back_A;
                    timer2.Start();
                }
            }
            else if(elevatorB.getButtonList().Contains(button))
            {
                int oldLocationB = elevatorB.getLocation();
                elevatorB.SendElevatorToFloor(button);

                if (elevatorB.getLocation() > oldLocationB)
                {
                    statusLabel.Text = "Elevator B is going Up";
                    timercounter = oldLocationB;
                    timer3.Interval = 1500;
                    timer3.Tick += Timer_Tick_B;
                    timer3.Start();

                }
                else if (elevatorB.getLocation() < oldLocationB)
                {
                    statusLabel.Text = "Elevator B is going Down";
                    timercounter = oldLocationB;
                    timer4.Interval = 1500;
                    timer4.Tick += Timer_Tick_Back_B;
                    timer4.Start();
                }
            }
            
        }

        private void Timer_Tick_A(object sender, EventArgs e)
        {
            elevatorCounterA.Text = (timercounter++).ToString();

            if (timercounter == elevatorA.getLocation()+1)
            {
                timercounter = 0;
                timer.Stop();
            }
        }
        private void Timer_Tick_Back_A(object sender, EventArgs e)
        {
            
            elevatorCounterA.Text = (timercounter--).ToString(); ;

            if (timercounter == elevatorA.getLocation()-1)
            {
                timercounter = 0;
                timer2.Stop();
            }
        }

        private void Timer_Tick_B(object sender, EventArgs e)
        {
            elevatorCounterB.Text = (timercounter++).ToString();

            if (timercounter > elevatorB.getLocation() )
            {
                timercounter = 0;
                timer3.Stop();
            }
        }
        private void Timer_Tick_Back_B(object sender, EventArgs e)
        {
            elevatorCounterB.Text = (timercounter--).ToString(); ;

            if (timercounter < elevatorB.getLocation() )
            {
                timercounter = 0;
                timer4.Stop();
            }
        }


    }
}
