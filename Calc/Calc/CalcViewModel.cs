using System;
using System.ComponentModel;

using System.Windows.Input;

using Xamarin.Forms;

namespace Calc
{
    class CalcViewModel : INotifyPropertyChanged

    {

        //아래 두 필드는 속성으로 구현되어 있다.


        //출력될 문자들을 담아둘 변수


        string inputString = "";




        //계산기화면의 출력 텍스트박스에 대응되는 필드


        string displayText = "";




        //속성이 바뀔때 이벤트 발생하도록 이벤트 정의


        public event PropertyChangedEventHandler PropertyChanged;




        //생성자


        public CalcViewModel()

        {

            //이벤트 핸들러 정의


            //숫자 버튼을 클릭할 때 실행


            this.AddCharCommand = new Command<string>((key) =>

            {




                this.InputString += key;

            });




            //백스페이스 버튼을 클릭할 때 실행, 한글자 삭제


            this.DeleteCharCommand = new Command((nothing) =>

            {

                this.InputString = this.InputString.Substring(0,

                                    this.InputString.Length - 1);

            },

                (nothing) =>

                {

                    // Return true if there's something to delete.

                    return this.InputString.Length > 0;

                });




            //Clear 버튼을 클릭할 때 실행, 출력창을 전부 지운다.


            this.ClearCommand = new Command((nothing) =>

            {

                // Clear


                this.InputString = "";

            });




            //+,-,*,/ 버튼을 클릭할 때 실행


            //현재출력창의 숫자를 Op1 속성에 저장, Op속성에 클릭한 연산자 저장


            //계산기의 출력화면을 Clear


            this.OperationCommand = new Command<string>((key) =>

            {

                this.Op = key;

                this.Op1 = Convert.ToDouble(this.InputString);

                this.InputString = "";

            });




            // =버튼을 클릭시 실행


            this.CalcCommand = new Command<string>((nothing) =>

            {

                this.Op2 = Convert.ToDouble(this.InputString);




                switch (this.Op)

                {

                    case "+": this.InputString = (this.Op1 + this.Op2).ToString(); break;

                    case "-": this.InputString = (this.Op1 - this.Op2).ToString(); break;

                    case "*": this.InputString = (this.Op1 * this.Op2).ToString(); break;

                    case "/": this.InputString = (this.Op1 / this.Op2).ToString(); break;

                }

            });

        }




        // Public 속성들을 정의


        public string InputString

        {

            protected set

            {

                if (inputString != value)

                {

                    inputString = value;

                    OnPropertyChanged("InputString");

                    this.DisplayText = inputString;




                    // 숫자버튼을 클릭하면 백스페이스 버튼을 활성화 시킨다.


                    ((Command)this.DeleteCharCommand).ChangeCanExecute();

                }

            }




            get { return inputString; }

        }




        //계산기의 출력창과 바인딩된 속성


        public string DisplayText

        {

            protected set

            {

                if (displayText != value)

                {

                    displayText = value;

                    OnPropertyChanged("DisplayText");

                }

            }

            get { return displayText; }

        }




        public string Op { get; set; }

        public double Op1 { get; set; }

        public double Op2 { get; set; }




        // 숫자 클릭


        public ICommand AddCharCommand { protected set; get; }




        // <- 클릭, 한글자씩 삭제


        public ICommand DeleteCharCommand { protected set; get; }




        // C 클릭시 DisplayText 전체를 지운다.


        public ICommand ClearCommand { protected set; get; }




        // +, -, *, / 클릭


        public ICommand OperationCommand { protected set; get; }




        // = 클릭


        public ICommand CalcCommand { protected set; get; }






        protected void OnPropertyChanged(string propertyName)

        {

            //이벤트를 발생시킨다.


            if (PropertyChanged != null)

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
