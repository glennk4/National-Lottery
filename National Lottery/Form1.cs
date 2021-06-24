using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace National_Lottery
{
    public partial class Form1 : Form
    {
        const int PICKABLE_NUMS = 48; 
        const int ALLOWED_NUMS = 6;
        const string SPACE = ",       ";
        const decimal THREEPRIZE = 30.0m;
        const decimal FOURPRIZE = 140.0m;
        const decimal FIVEPRIZE = 1750.0m;
        const decimal FIVE_BONUS = 1000000.0m; 
        const decimal SIXPRIZE = 12523601.0m; 

        int matches = 0;
        bool bonus = false; 
        int[] playerNums = new int[ALLOWED_NUMS];
        int picks =0; 

        public Form1()
        {
            InitializeComponent();
        }

//Once user confirms, checks that the desired amount of picks have been made, generates results if so and message to users on error. 
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            ConfirmSelection(ref playerNums, ref picks);

            if (NumbersGood(playerNums, picks))
            {
                GenerateNumbers(ref playerNums);
                CheckWinnings(ref matches, ref bonus); 
            }
            else
            {
                MessageBox.Show("Please check your numbers. There should be SIX numbers selected");
                picks = 0; 
            }
        }


// Takes the player numbers, generates drawn numbers and outputs to user. 
        private void GenerateNumbers(ref int[] playerNums)
        {
            int[] resultNo = new int[7];
            string[] outputResult = new string[7];
            Random rand = new Random();


            for (int i = 0; i < resultNo.Length; i++)
            {
                int testNumber = rand.Next(1, PICKABLE_NUMS);
                resultNo[i] = testNumber;
            }

            CheckForDuplicates(ref resultNo); 
            SortNumbers(ref resultNo);
            CheckMatches(ref playerNums, ref resultNo, ref bonus, ref matches);

            CheckWinnings(ref matches, ref bonus); 

            for (int i = 0; i < resultNo.Length; i++)
            {
                outputResult[i] = resultNo[i].ToString(); 
            }

            tempResults.Text = outputResult[0] +SPACE+ outputResult[1] +SPACE+ outputResult[2] +SPACE+
                outputResult[3] +SPACE+ outputResult[4] +SPACE+ outputResult[5] + SPACE+ " BONUS BALL:  " +
                outputResult[6];
        }


        //Confirms user has made six selections
        private void ConfirmSelection(ref int[] playerNums, ref int picks)
        {
 
            foreach (Control c in NumberPanel.Controls)
            {
                if ((c as CheckBox).Checked)
                {
                    if (picks < ALLOWED_NUMS)
                    {
                        playerNums[picks] = int.Parse(c.Text);
                    }
                    picks++;
                }   
            }

            NumbersGood(playerNums, picks); 

        }

        //Bool check to ensure no values are null as a result of too few picks made 
        private bool NumbersGood(int[] numbers, int picks)
        {
            bool numsGood = false;
            int index = 0;

            while (index < numbers.Length)
            {
                if (numbers[index] == 0||picks>6)
                {
                    numsGood = false;
                    index = numbers.Length;
                }
                else
                { 
                    numsGood = true;
                    index++;
                }
            }
            return numsGood;
        }


//Takes the result array and sorts into ascending order 
        private void SortNumbers(ref int[] array)
        {
            int minIndex;
            int minValue;

            for (int startCheck = 0; startCheck < array.Length-1; startCheck++)
            {
                minIndex = startCheck;
                minValue = array[startCheck];

                for (int i = startCheck + 1; i < array.Length-1; i++)
                {
                    if (array[i] < minValue)
                    {
                        minValue = array[i];
                        minIndex = i; 
                    }
                }

                int tempNo = array[minIndex];
                array[minIndex] = array[startCheck];
                array[startCheck] = tempNo;
            }
        }

//Checks the random generated results array for any duplicates - if found, the number is replaced with another and loop starts again  
        private void CheckForDuplicates(ref int[] resultNo)
        {

            for (int i = 0; i < resultNo.Length; i++) {

                for (int checkPos = i + 1; checkPos < resultNo.Length; checkPos++)
                {
                    if (resultNo[i] == resultNo[checkPos])
                    {
                        Random rand = new Random();
                        resultNo[checkPos] = rand.Next(1, PICKABLE_NUMS); 
                        i = 0;
                        checkPos = 0; 
                    }

                }
            }
        }


//Checks the users selected numbers against the winning result numbers 
        private void CheckMatches(ref int[] playerNums, ref int[] resultNo, ref bool bonus, ref int matches)

        {
            for (int i = 0; i < playerNums.Length; i++)
            {
                if (playerNums[i] == resultNo[6])
                {
                    bonus = true;
                }
            }

            for (int i = 0; i < playerNums.Length; i++)

                for (int checkedNo = 0; checkedNo < playerNums.Length; checkedNo++)
                {
                    {
                        if (playerNums[i] == resultNo[checkedNo])
                        {
                            matches++;
                        }
                    }
                }
        }


//Outputs the amount of matches and if applicable, prize amount to the user 
        private void CheckWinnings(ref int matches, ref bool bonus)
        {
            WinningPanel.Visible = true; 

            switch (matches) {
                case 0:
                MatchesOutput.Text = "No matches"; 
                break;
                case 1:
                MatchesOutput.Text = "1 number matched";
                break;
                case 2:
                MatchesOutput.Text = "2 numbers matched";
                break;
                case 3:
                MatchesOutput.Text = "3 numbers matched";
                PrizeOutput.Text = "Congratulations! You have won " + THREEPRIZE.ToString("C");
                PrizeOutput.Visible = true; 
                break;
                case 4:
                MatchesOutput.Text = "4 numbers matched";
                PrizeOutput.Text = "Congratulations! You have won " + FOURPRIZE.ToString("C");
                PrizeOutput.Visible = true;
                break;
                case 5:
                
                if(bonus)
                    { 
                    MatchesOutput.Text = "5 numbers plus bonus matched";
                    PrizeOutput.Text = "Congratulations! You have won " + FIVE_BONUS.ToString("C");
                    PrizeOutput.Visible = true;
                    }
                else
                {
                    MatchesOutput.Text = "5 numbers matched";
                    PrizeOutput.Text = "Congratulations! You have won " + FIVEPRIZE.ToString("C");
                    PrizeOutput.Visible = true;
                    }
                    break;
                case 6:
                MatchesOutput.Text = "You have matched all 6 numbers";
                PrizeOutput.Text = "Congratulations! You have won " + SIXPRIZE.ToString("C");
                PrizeOutput.Visible = true;
                break; 
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Application.Restart(); 
        }
    }
}