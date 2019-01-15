using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOLP
{
    public class Stolp
    {
        int sizeE = -1;
        public static double metrics(Data firstPoint, Data secondPoint)
        {
            double sum = 0;
            for(int i = 0; i < Data.MaxAttributes; ++i)
            {
                sum += Math.Pow(firstPoint.Attributes[i] - secondPoint.Attributes[i], 2);
            }
            return Math.Sqrt(sum);
        }


        public List<Data> emissionСutOff(List<Data> sample, double delta)
        {
            double sameClass = 0;
            double anotherClass = 0;
            List<Data> list = new List<Data>();
            //Console.WriteLine (delta);
            for (int i = 0; i < sample.Count; i++)
            {
                for (int j = 0; j < sample.Count; j++)
                {
                    if (metrics(sample[i], sample[j]) != 0)
                    {
                        if (sample[i].ObjClass == sample[j].ObjClass)
                        {
                            sameClass += 1 / metrics(sample[i], sample[j]);
                        }
                        else
                            anotherClass += 1 / metrics(sample[i], sample[j]);
                    }
                }
                if (sameClass - anotherClass > delta)
                {
                    list.Add(sample[i]);
                }
                sameClass = 0;
                anotherClass = 0;
            }
            return list;
        }

        public List<Data> findStandard(List<Data> sample)
        {
            double sameClass = 0;
            double anotherClass = 0;
            double maxIndent = 0;
            double maxIndent2 = 0;
            Data standard = new Data(new double[] { 0, 0 }, 0);
            Data standard2 = new Data(new double[] { 0, 0 }, 0);
            int x = 10;
            for (int i = 0; i < sample.Count; i++)
            {
                for (int j = 0; j < sample.Count; j++)
                {
                    if (metrics(sample[i], sample[j]) != 0)
                    {
                        if (sample[j].ObjClass == sample[i].ObjClass)
                        {
                            if (1 / metrics(sample[i], sample[j]) < x)
                                sample[i].weight += 1 / metrics(sample[i], sample[j]);
                            else sample[i].weight += x;
                        }
                        else
                        {
                            if (1 / metrics(sample[i], sample[j]) < x)
                                anotherClass += 1 / metrics(sample[i], sample[j]);
                            else anotherClass += x;
                            //Console.WriteLine("2:" + anotherClass);
                        }
                    }
                }
                sample[i].weight -= anotherClass;
                /*if (sample[i].ObjClass == 0)
                {
                    if (maxIndent < (sameClass - anotherClass))
                    {
                        maxIndent = (sameClass - anotherClass);
                        //Console.WriteLine (maxIndent);
                        standard = sample[i];
                    }
                }
                else
                {
                    if (maxIndent2 < (sameClass - anotherClass))
                    {
                        maxIndent2 = (sameClass - anotherClass);
                        //Console.WriteLine (maxIndent);
                        standard2 = sample[i];
                    }
                }*/
                sameClass = 0;
                anotherClass = 0;
            }
            for (int i = 0; i < sample.Count; ++i)
            {
                if (sample[i].ObjClass == 0 && maxIndent < sample[i].weight)
                {
                    maxIndent = sample[i].weight;
                    standard = sample[i];
                }
                else if (sample[i].ObjClass == 1 && maxIndent2 < sample[i].weight)
                {
                    maxIndent2 = sample[i].weight;
                    standard2 = sample[i];
                }
            }
            List<Data> standarts = new List<Data> { standard, standard2 };
            return standarts;
        }


        /*public List<Data> findStandard(List<Data> sample)
        {
            double sameClass = 0;
            double anotherClass = 0;
            double maxIndent = 0;
            double maxIndent2 = 0;
            Data standard = new Data(new double[] { 0, 0 }, 0);
            Data standard2 = new Data(new double[] { 0, 0 }, 0);
            for (int i = 0; i < sample.Count; i++)
            {
                for (int j = 0; j < sample.Count; j++)
                {
                    if (metrics(sample[i], sample[j]) != 0)
                    {
                        if (sample[j].ObjClass == 0)
                        {
                            sameClass += 1 / metrics(sample[i], sample[j]);
                        }
                        else
                        {
                            anotherClass += 1 / metrics(sample[i], sample[j]);
                        }
                    }
                }
                if (maxIndent < (sameClass - anotherClass))
                {
                    maxIndent = (sameClass - anotherClass);
                    //Console.WriteLine (maxIndent);
                    standard = sample[i];
                }
                if (maxIndent2 < (anotherClass - sameClass))
                {
                    maxIndent2 = (anotherClass - sameClass);
                    //Console.WriteLine (maxIndent);
                    standard2 = sample[i];
                }
                sameClass = 0;
                anotherClass = 0;
            }
            List<Data> standarts = new List<Data> { standard, standard2 };
            return standarts;
        }*/

        public Data errors(List<Data> sample, List<Data> omega)
        {
            List<Data> sampleWithoutOmega = new List<Data>();
            bool check = false;
            sizeE = 0;
            for (int i = 0; i < sample.Count; i++)
            {
                for (int j = 0; j < omega.Count; j++)
                {
                    if (omega[j] == sample[i])
                        check = true;
                }
                if (check == false)
                    sampleWithoutOmega.Add(sample[i]);
                check = false;
            }

            double sameClass = 0;
            double anotherClass = 0;
            Data minError = new Data(new double[] { 0, 0}, 0);
            double min = int.MaxValue;
            List<Data> newOmega = new List<Data>();
            int x = 100;
            for (int i = 0; i < sampleWithoutOmega.Count; i++)
            {
                for (int j = 0; j < omega.Count; j++)
                {
                    if (metrics(sampleWithoutOmega[i], omega[j]) != 0)
                    {

                        if (omega[j].ObjClass == sampleWithoutOmega[i].ObjClass)
                        {
                            if (1 / metrics(sampleWithoutOmega[i], omega[j]) < x)
                                sameClass += 1 / metrics(sampleWithoutOmega[i], omega[j]);
                            else sameClass += x;
                            //sameClass += 1 / metrics(sampleWithoutOmega[i], omega[j]);
                        }
                        else
                        {
                            if (1 / metrics(sampleWithoutOmega[i], omega[j]) < x)
                                anotherClass += 1 / metrics(sampleWithoutOmega[i], omega[j]);
                            else anotherClass += x;
                            //anotherClass += 1 / metrics(sampleWithoutOmega[i], omega[j]);
                        }
                    }
                }
                //Console.WriteLine (sameClass - anotherClass);
                if (0 > (sameClass - anotherClass))
                {
                    sizeE++;
                    if ((sameClass - anotherClass) < min)
                    {
                        min = (sameClass - anotherClass);
                        minError = sampleWithoutOmega[i];
                    }
                    //Console.WriteLine (sizeE);
                }
                
                sameClass = 0;
                anotherClass = 0;
                //Console.WriteLine (sizeE);
            }
            if (min == int.MaxValue)
                return null;
            return minError;
        }

        public List<Data> stolp(List<Data> sample, double delta, double errorsPercent)
        {
            if (errorsPercent < 1)
                errorsPercent = 1;
            List<Data> cutOff = new List<Data>();
            cutOff = emissionСutOff(sample, delta);
            List<Data> omega = new List<Data>();
            omega.AddRange(findStandard(cutOff));
            while (omega.Count != sample.Count)
            {
                Data d = errors(cutOff, omega);
                if(d != null)
                    omega.Add(errors(cutOff, omega));
                if (sizeE < errorsPercent)
                    return omega;
            }
            return omega;
        }

        public int classifier(List<Data> omega, Data newPoint)
        {
            double sameClass = 0;
            double anotherClass = 0;
            for (int j = 0; j < omega.Count; j++)
            {
                if (metrics(newPoint, omega[j]) != 0)
                {
                    if (omega[j].ObjClass == 1)
                    {
                        sameClass += 1 / metrics(newPoint, omega[j]);
                    }
                    else
                    {
                        anotherClass += 1 / metrics(newPoint, omega[j]);
                    }
                }
            }
            if (0 < (sameClass - anotherClass))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
