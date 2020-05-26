using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new TFGraph();
            //重点是下面的这句，把训练好的pb文件给读出来字节，然后导入
            var model = File.ReadAllBytes("model.pb");
            graph.Import(model);
            //传进来的double数组
           
            using (var sess = new TFSession(graph))
            {
                var runner = sess.GetRunner();
                //获取预测对象
                var pre = graph["pre"][0];
                //获取数据归一化的均值
                var GetMean = graph["mean"][0];
                var mean = runner.Run(GetMean);
                var datamean = (float[,])mean.GetValue();
                //获取数据归一化的方差
                var GetStd = graph["std"][0];
                var std = runner.Run(GetStd);
                var datastd = (float[,])std.GetValue();
                var shape = new long[] { 1,18 };
                var size = 18 * sizeof(float);
                var input = new TFTensor(TFDataType.Float, shape, size);
                runner.AddInput(graph["X"][0], input);
                Console.WriteLine("输入预测需要的18个数据，数值之间用空格隔开！");
                while (true)
                {
                    Console.WriteLine("等待输入……");
                    string str = Console.ReadLine();
                    //string str = "379,68,610.8,45,27486,410,45	797,415,0	,399,	24,40,17,	1	,1	,1	,110";
                    string[] strarray = str.Split(' '); 
                    double[,] dou = new double[1, 18];
                    for (int i = 0; i < strarray.Length; i++)
                    {
                        dou[0, i] = Convert.ToDouble(strarray[i]);
                    }
                    //将double类型的数组转化为float类型
                    float[,] floatitem = ConvertDoubleArrayTofloat(dou);
                    var inputs = new float[][,] { floatitem };
              
                    //定义空的输入张量
              
                    //进行数据的归一化  （data-mean）/std
                    var input0 = inputs[0];
                    for (int i = 0; i < 18; i++)
                    {
                        input0[0, i] = (input0[0, i] - datamean[0, i]) / datastd[0, i];
                    }

                    inputs[0] = input0;
                    input.SetValue(input0);


                  
                    var r = runner.Run(graph["pre"][0]);
                    var v1 = (float[,])r.GetValue();
                    var v = ConvertfloatArrayToOneOtZero(v1);
                    Console.WriteLine("预测故障为：");
                    Console.Write(v[0, 0]);
                    Console.Write(v[0, 1]);
                    Console.Write(v[0, 2]);
                    Console.Write(v[0, 3]);
                    Console.Write(v[0, 4]);
                    Console.WriteLine();


                }
              
            }

        }
        public static float[,] ConvertDoubleArrayTofloat(double[,] array)
        {
            var row = array.GetLength(0);
            var clo = array.GetLength(1);
            float[,] arrayfloat = new float[1, 18];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < clo; j++)
                {
                    arrayfloat[i, j] = (float)array[i, j];
                }
            }
            return arrayfloat;
        }
        public static int[,] ConvertfloatArrayToOneOtZero(float[,] array)
        {
            var row = array.GetLength(0);
            var clo = array.GetLength(1);
            int[,] arrayresult = new int[1, 5];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < clo; j++)
                {
                    if (array[i, j] > 0.5)
                    {
                        arrayresult[i, j] = 1;
                    }
                    else
                    {
                        arrayresult[i, j] = 0;
                    }
                }

            }
            return arrayresult;
        }
    }
}
