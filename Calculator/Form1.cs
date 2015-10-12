using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        // This keeps the one of the four arithmetic operators in memory
        private String operator_sign = "";

        // This stores the before value of the arthimatic operation. So in case of 1+3 , this would store 1
        private double before_value = 0;

        // True when one of the arithmetic operation buttons is pressed
        private bool arithmtic_clicked = false;

        // Checks if the reciprocal button had been pressed
        private bool isReciprocalPressed = false;

        // Used for storing the string from text that exceeds the textbox size 
        private string wholeEquation = "";

        // Checks whether text in textbox had reached a certain value after which it will go off the screen
        private bool maxSizeReached = false;

        private String rest = "";

        public Calculator()
        {
            InitializeComponent();
        }

        // This function is for all numeric buttons
        private void button_Click(object sender, EventArgs e)
        {
            // Clears the screen in order to append 0 at the beginning for example 01 and also to avoid arithematic exceptions 
            if (result.Text == "0" || arithmtic_clicked)
                result.Clear();

            // Sets false so values after the arithmetic operator in equation could be appended on the screen
            this.arithmtic_clicked = false;

            // Casting object recieved to button object
            Button button = (Button)sender;

            // Appends the value on screen with the button text
            result.Text = result.Text + button.Text;




        }

        // This function is for CE button
        private void Clear(object sender, EventArgs e)
        {
            // Clears the screen in case an undesired number was entered previously
            result.Clear();

            result.Text = "0";
        }

        //This function is for the . button
        private void dot(object sender, EventArgs e)
        {
            // The if loop prevents . being appended more than once to a numeric value on screen
            if (result.Text.IndexOf(".") == -1)
                result.Text = result.Text + ".";
        }

        // This function is for the four arithematic operators
        private void arithmetic_operation(object sender, EventArgs e)
        {
            // In case there is nothing on the screen
            if (result.Text.Length == 0)
                result.Text = "0";

            Button button = (Button)sender;

            // In case reciprocal was pressed . It works differently than other operators 
            if (button.Text == "1/x")
            {
                string temp = result.Text;

                // Takes the reciprocal of the number on the screen
                result.Text = Math.Pow(double.Parse(result.Text), -1).ToString();
                
                // Used for truncating the outcome in the label not on the screen
                if (result.Text.IndexOf(".") != -1)
                {
                    if (result.Text.Substring(result.Text.IndexOf("."), result.Text.Length - result.Text.IndexOf(".")).Length > 5)
                        eq.Text = eq.Text + "reciprocal(" + temp + ")";
                    else
                        eq.Text = eq.Text + result.Text;

                }
                else
                {
                    eq.Text = eq.Text + result.Text;
                }
                
                this.isReciprocalPressed = true;
            }
            

            else {

                //If the operator button had been pressed previously
                if (this.operator_sign != "")
                {



                    if (this.wholeEquation.Length > 0 && this.maxSizeReached)
                    {
                        this.rest = this.wholeEquation;
                        this.wholeEquation = "";
                    }
                    // Calls the function which is used when = is pressed
                    this.equality(sender, e);

                    this.rest = "";

                    //Button button = (Button)sender;

                    // Stores the latest operator button pressed
                    this.operator_sign = button.Text;

                    this.arithmtic_clicked = true;

                    // For the label
                    eq.Text = eq.Text + button.Text;

                    //MessageBox.Show("After wholeEquation : " + this.wholeEquation);

                }
                else
                {
                    // Converts the string on the screen into double and stores it
                    this.before_value = double.Parse(result.Text);

                    // Changes the screen by appending the operator pressed to it so for example 1 becomes 1+
                    result.Text = result.Text + button.Text;

                    this.operator_sign = button.Text;

                    this.arithmtic_clicked = true;

                    // For the label
                    eq.Text = eq.Text + result.Text;
                }
            }
            
        }

        // This function is used when equal button is pressed
        private void equality(object sender, EventArgs e)
        {
            if (result.Text.Length == 0)
                result.Text = "0";

            // Converts the value on the screen which is after the arithematic operator to double and stores it
            double after_value;

            //Catches exception in case user presses = button prematurely
            try
            {
                    after_value = double.Parse(result.Text);
            }
            catch (System.FormatException)
            {
                // In windows calculator , in situations like 23+ and then if = is pressed answer becomes 46
                after_value = this.before_value;
            }
            // Performs the arithematic operation according to the sign stored in the private variable below
            switch(this.operator_sign)
            {
                case "+":
                    result.Text = (this.before_value + after_value).ToString();
                    break;
                case "-":
                    result.Text = (this.before_value-after_value).ToString();
                    break;
                case "*":
                    result.Text = (this.before_value * after_value).ToString();
                    break;
                case "/":
                    result.Text = (this.before_value / after_value).ToString();
                    break;
                case "^":
                    result.Text = (Math.Pow(this.before_value, after_value)).ToString();
                    break;
                case "^1/x":
                    result.Text = (Math.Pow(before_value,Math.Pow(after_value,-1))).ToString();
                    break;
                default:
                    break;
            }

            // In case numbers go off the screen
            if (this.maxSizeReached)
            {
                this.before_value = double.Parse(this.wholeEquation + result.Text);
               
            }
            // This is done so a continous chain of calculations could be performed just by pressing the operators
            else
                this.before_value = double.Parse(result.Text);

            
            // Prevents from appending the number twice on the screen
            if (!isReciprocalPressed)
            {
              
                eq.Text = eq.Text + after_value;
            }
            else
                this.isReciprocalPressed = false;

            Button button = (Button)sender;

            //Actually pressing = button marks the end of equation
            // Resets everything except the before_value because that is the result on the screen
            if(button.Text=="=")
            {
                this.operator_sign = "";
                this.arithmtic_clicked = false;
                eq.Text="";
                this.wholeEquation = "";
                this.maxSizeReached = false;
                this.isReciprocalPressed = false;
            }

            
        }

        // This function is for C button . It basically clears everything and resets everything
        private void button17_Click(object sender, EventArgs e)
        {
            result.Text = "0";
            this.before_value = 0;
            this.operator_sign = "";
            this.arithmtic_clicked = false;
            this.eq.Text = "";
            this.wholeEquation = "";
            this.maxSizeReached = false;
            this.isReciprocalPressed = false;
        }

        // This function is used so values could be entered from the keyboard.It has only some of the keys 
        private void Calculator_KeyPress(object sender, KeyPressEventArgs e)
        {
     
            switch (e.KeyChar.ToString())
            {
                case "1":
                    button1.PerformClick();
                    break;
                case "2":
                    button2.PerformClick();
                    break;
                case "3":
                    button3.PerformClick();
                    break;
                case "4":
                    button4.PerformClick();
                    break;
                case "5":
                    button5.PerformClick();
                    break;
                case "6":
                    button6.PerformClick();
                    break;
                case "7":
                    button7.PerformClick();
                    break;
                case "8":
                    button8.PerformClick();
                    break;
                case "9":
                    button9.PerformClick();
                    break;
                case "0":
                    button13.PerformClick();
                    break;
                case ".":
                    button14.PerformClick();
                    break;
                case "/":
                    button10.PerformClick();
                    break;
                case "*":
                    button11.PerformClick();
                    break;
                case "-":
                    button12.PerformClick();
                    break;
                case "+":
                    button15.PerformClick();
                    break;
                case "=":
                    button16.PerformClick();
                    break;
                default:
                    break;


            }
        }

        // Function is called when the textsize in the eq changes. It is used so that numbers could go off screen
        private void eq_TextChanged(object sender, EventArgs e)
        {
            // 17 is the number of chars the label can take at it's max at it's current size
            if (eq.Text.Length > 17)
            {
                // Cuts off the left-most char in order to make room for the incoming right ones
                eq.Text = eq.Text.Substring(1, eq.Text.Length - 1);
            }
            else
            {
                if (this.maxSizeReached && this.wholeEquation.Length<17)
                {
                    

                    try
                    {
                        
                        eq.Text =(this.rest+this.wholeEquation+result.Text).Substring(0,17);
                        
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        //MessageBox.Show("Throws an error");
                    }
                }
            }

        }

        // This function is called when <- button is clicked
        private void Erase(object sender, EventArgs e)
        {
            // Prevents from null exception
            if (result.Text.Length == 0)
                result.Text = "0";

            else
            {
                //maxSizeReached becomes true when the function below is called first and length of the textBox text > 17
                if (this.maxSizeReached)
                {
                    // Cuts off the right most char of the text in the textBox
                    result.Text = result.Text.Substring(0, result.Text.Length - 1);

                    // Once there is are no chars off the screen
                    if (this.wholeEquation.Length == 0)
                        this.maxSizeReached = false;
                    else
                    {
                        // Appends the right most char of the screen onto the left most side of the text on the screen 
                        result.Text = this.wholeEquation.Substring(this.wholeEquation.Length - 1, 1) + result.Text;

                        // Substracts the char that was added above
                        this.wholeEquation = this.wholeEquation.Substring(0, this.wholeEquation.Length - 1);
                    }

                }
                // Keep cutting off the right most char from the screen
                else
                    result.Text = result.Text.Substring(0, result.Text.Length - 1);
            }
        }

        // This function is called each time size of the text in the textBox changes
        private void result_TextChanged(object sender, EventArgs e)
        {
            
            // The textBox can withhold at most 13 chars
            if (result.Text.Length > 13)
            {
                // The char that is about to be cut-off is stored in the string below
                this.wholeEquation = this.wholeEquation+result.Text.Substring(0,1);
                //MessageBox.Show(" Down this.wholeEquation :" + this.wholeEquation);
                
                // Left most char from the screen is cut
                result.Text = result.Text.Substring(1, result.Text.Length - 1);

                this.maxSizeReached = true;
            }
            

        }

        
    }
}
