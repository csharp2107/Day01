using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadConsoleExample
{

    public delegate void ResultCallbackDelegate(int result);

    class SumHelper
    {
        private int number;
        private ResultCallbackDelegate resultCallback;

        public SumHelper(int number, ResultCallbackDelegate resultCallback)
        {
            this.number = number;
            this.resultCallback = resultCallback;
        }

        public void CalculationSum()
        {
            int result = 0;
            for (int i = 1; i <= number; i++)
            {
                result += i;
            }

            if (resultCallback != null)
            {
                resultCallback(result);
            }
        }

    }
}
