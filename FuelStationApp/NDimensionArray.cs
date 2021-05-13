// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.Serialization;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace Lab4
// {
//     public interface IMArray<T>
//     {
//         public int[,] IndicesOf(T a);
//         public void ClearRange(params int[] a);
//     }
//
//     [DataContract]
//     public abstract class AbstractMultidimensionalArray<T>
//     {
//         public virtual Tuple<int[]> Shape { get; }
//         public virtual T[] LineArray { get; set; }
//     }
//
//     [DataContract]
//     public class NDimensionArray<T> : AbstractMultidimensionalArray<T>, ICloneable, IEquatable<NDimensionArray<T>>,
//         IEnumerable<T>, IMArray<T> where T : IComparable<T> // тип Т повинен підтримувати інтерфейс IComparable<T> 
//     {
//         [DataMember] protected T[] mas; // масив іх елементів
//         // plurality in names: dimensions, ...
//         [DataMember] private int[] dimension; // зберігаєм кількість вимірів
//
//         [DataMember]
//         private int[] sizeDimension; // зберігаєм скільки елементів потрібно, щоб заповнити 1 для кожного виміру
//
//
//         public override T[] LineArray
//         {
//             get
//             {
// // => mas
// // may be better solution                
//                 T[] newMas = (T[]) mas.Clone();
//                 return newMas;
//             }
//
//             set
//             {
//                 if (value.GetType() == mas.GetType() && value.Length == mas.Length)
//                 {
//                     mas = (T[]) value.Clone();
//                 }
//             }
//         }
//
//
//         public override Tuple<int[]> Shape
//         {
//             get
//             {
//                 int[] copyDimension = (int[]) dimension.Clone();
//                 var a = Tuple.Create(copyDimension);
//                 return a;
//             }
//         }
//
//
//         public void Reverse()
//         {
//             Array.Reverse(mas);
//             Console.WriteLine(mas[0]);
//         }
//
//
//         public NDimensionArray(params int[] shape)
//         {
//             dimension = (int[]) shape.Clone();
//             int it = 1; // кількість елементів
//             foreach (int i in shape)
//             {
//                 if (i <= 0) throw new ArgumentException();
//                 it *= i;
//             }
//
//             mas = new T[it]; // ініціалізуєм одновимірний масив розмірністю виходячи з кількості елементів
//             int p = 1; // Підрахунок кількості елементів для кожного рівня
//             sizeDimension = new int[shape.Length];
//             for (int i = shape.Length - 1; i >= 0; i--)
//             {
//                 sizeDimension[i] = p;
//                 p *= shape[i];
//             }
//         }
//
//
//         public NDimensionArray()
//         {
//             dimension = new int[2] {100, 100};
//             mas = new T[10000];
//             sizeDimension = new int[2] {100, 1};
//         }
//
//
//         /// <summary>
//         /// Створює глибоку копію даного обєкта і повертає її в місце виклику
//         /// </summary>
//         /// <returns>копія даного обєкта</returns>
//         public object Clone()
//         {
//             NDimensionArray<T> array = new NDimensionArray<T>(dimension);
//             array.mas = (T[]) mas.Clone();
//             return array;
//         }
//
//
//         /// <summary>
//         /// Створюється строка яка містить представлення основних полів класу
//         /// </summary>
//         /// <returns>Відформатована строка</returns>
//         public override string ToString()
//         {
//             string a1 = "[";
//             string a2 = "]";
//
//             var dimensionToSt = (int[]) dimension.Clone();
//             string[] stArray = new string[dimension.Length];
//             stArray[0] = "\n";
//             for (int i = 1; i < stArray.Length; i++)
//             {
//                 stArray[i] = stArray[i - 1] + "\t";
//             }
//
//
//             string stres;
//             if (dimension.Length == 1)
//             {
//                 stres = "[]";
//             }
//             else
//             {
//                 stres = stArray[dimensionToSt.Length - 2] + "1" + a1 + a2 + "1";
//
//                 for (int i = dimensionToSt.Length - 2; i >= 0; i--)
//                 {
//                     string str = stres;
//                     while (dimensionToSt[i] != 1)
//                     {
//                         stres += str;
//                         dimensionToSt[i]--;
//                     }
//
//                     if (i != 0)
//                         stres = (stArray[i - 1] + a1) + stres + (stArray[i - 1] + a2);
//                 }
//             }
//
//             int iterator = 0;
//             string[] masSt = stres.Split(new[] {'1'}, StringSplitOptions.RemoveEmptyEntries);
//             for (int i = 0; i < masSt.Length; i++)
//             {
//                 if (masSt[i] == "[]")
//                 {
//                     string s = "[";
//                     int y = dimension[dimension.Length - 1];
//                     for (int j = 0; j < y; j++)
//                     {
//                         s += (mas[iterator] is null) ? " " : mas[iterator].ToString();
//                         if (j != y - 1) s += ",";
//                         iterator++;
//                     }
//
//                     s += "]";
//                     masSt[i] = s;
//                 }
//             }
//
//             return String.Join("", masSt);
//         }
//
//
//         /// <summary>
//         /// Перетворює заданий мавив індексів у лінійний індекс виходячи з розмірності даного масива
//         /// </summary>
//         /// <param name="mas"> масив індексів відповідного елемента</param>
//         /// <returns>лінійний індекс елемента</returns>
//         protected int ConvertLinearIndex(int[] mas)
//         {
//             int y = 0;
//             for (int i = 0; i < mas.Length; i++)
//             {
//                 y += (mas[i] * sizeDimension[i]);
//             }
//
//             return y;
//         }
//
//
//         /// <summary>
//         /// Перевірка чи заданий інлекс не виходить за межі розмірностей даного багатовимірного масиву 
//         /// </summary>
//         /// <param name="index">масив індексів елемента, який перевіряється</param>
//         /// <returns>якщо індекс виходить за межі повертає true, інакше - false</returns>
//         protected bool flagCreating(int[] index)
//         {
//             bool flag = false;
//             if (index.Length != dimension.Length) flag = true;
//             else
//             {
//                 for (int i = 0; i < dimension.Length; i++)
//                 {
//                     if (dimension[i] < (index[i] + 1) || index[i] < 0)
//                     {
//                         flag = true;
//                         break;
//                     }
//                 }
//             }
//
//             return flag;
//         }
//
//
//         public void CopyTo(T[] array, params int[] index)
//         {
//             if (index.Length == this.dimension.Length)
//             {
//                 bool flag = flagCreating(index);
//                 int y = ConvertLinearIndex(index);
//
//                 if (array.Length < (mas.Length - y + 1)) flag = true;
//
//                 if (!flag)
//                 {
//                     int k = 0;
//                     for (int i = y; i < mas.Length; i++)
//                     {
//                         array[k] = mas[i];
//                         k++;
//                     }
//                 }
//                 else
//                     throw new IndexOutOfRangeException();
//             }
//         }
//
//
//         public void CopyTo(T[] array)
//         {
//             bool flag = false;
//             if (array.Length < mas.Length) flag = true;
//
//             if (!flag)
//             {
//                 for (int i = 0; i < mas.Length; i++)
//                 {
//                     array[i] = mas[i];
//                 }
//             }
//             else
//                 throw new IndexOutOfRangeException();
//         }
//
//
//         public void CopyTo(NDimensionArray<T> array, params int[] index)
//         {
//             if (this.dimension.SequenceEqual(array.dimension))
//             {
//                 int y = ConvertLinearIndex(index);
//
//                 for (int i = y; i < mas.Length; i++)
//                 {
//                     array.mas[i] = this.mas[i];
//                 }
//             }
//             else
//                 throw new IndexOutOfRangeException();
//         }
//
//
//         public void CopyTo(NDimensionArray<T> array)
//         {
//             if (this.dimension.SequenceEqual(array.dimension))
//             {
//                 array.mas = (T[]) mas.Clone();
//             }
//             else
//                 throw new IndexOutOfRangeException();
//         }
//
//
//         /// <summary>
//         /// Відбувається пошук в багатовимірному масиві, всіх елементів, які рівні заданому значенню
//         /// </summary>
//         /// <param name="a">значення яке шукається</param>
//         /// <returns>масив індексів</returns>
//         public int[,] IndicesOf(T a)
//         {
//             int ind = -1;
//             int k = 0;
//             while (true)
//             {
//                 ind = Array.IndexOf(this.mas, a, ind + 1);
//                 if (ind == -1) break;
//                 k++;
//             }
//
//             int[,] masRes = new int[k, dimension.Length];
//             ind = -1;
//
//             while (k != 0)
//             {
//                 ind = Array.IndexOf(this.mas, a, ind + 1);
//                 int t = ind;
//                 for (int j = 0; j < dimension.Length; j++)
//                 {
//                     masRes[k - 1, j] = t / sizeDimension[j];
//                     t -= sizeDimension[j] * masRes[k - 1, j];
//                 }
//
//                 k--;
//             }
//
//             return masRes;
//         }
//
//
//         /// <summary>
//         /// Відбувається перевірка екземплярів класу на рівність
//         /// </summary>
//         /// <param name="a">перший екземпляр класу</param>
//         /// <param name="b">другий екземпляр класу</param>
//         /// <returns>-1 якщо елемент 1 менше елементу 2, -1 якщо елемент 1 більше елементу 2, 0 якщо елемент 1 менше елементу 2</returns>
//         public static int Comparisons(NDimensionArray<T> a, NDimensionArray<T> b)
//         {
//             if (a is null && b is null) return 0;
//             if (a is null) return -1;
//             if (b is null) return 1;
//             if (a.mas.Length == b.mas.Length)
//             {
//                 for (int i = 0; i < a.mas.Length; i++)
//                 {
//                     if (a.mas[i] is null && b.mas[i] is null) continue;
//                     if (a.mas[i] is null || b.mas[i] is null || a.mas[i].CompareTo(b.mas[i]) != 0)
//                     {
//                         if (a.mas[i] is null) return -1;
//                         if (b.mas[i] is null) return 1;
//                         if (a.mas[i].CompareTo(b.mas[i]) != -1) return 1;
//                         else return -1;
//                     }
//                 }
//
//                 return 0;
//             }
//
//             if (a.mas.Length > b.mas.Length) return 1;
//             else return -1;
//         }
//
//
//         /// <summary>
//         /// Очищає проміжок значень починаючи і закінчуючи заданими індексами
//         /// </summary>
//         /// <param name="a">масив індексів початку та кінця</param>
//         public void ClearRange(params int[] a)
//         {
//             if (a.Length % 2 != 0) throw new ArgumentException();
//             var mas_1 = new int[a.Length / 2];
//             var mas_2 = new int[a.Length / 2];
//
//             for (int i = 0; i < a.Length; i++)
//             {
//                 if (i < a.Length / 2) mas_1[i] = a[i];
//                 else mas_2[i - a.Length / 2] = a[i];
//             }
//             //a.CopyTo(mas_2, a.Length / 2);
//
//             if (!flagCreating(mas_1) && !flagCreating(mas_2))
//             {
//                 int index1 = ConvertLinearIndex(mas_1);
//                 int index2 = ConvertLinearIndex(mas_2);
//                 for (int i = index1; i <= index2; i++)
//                 {
//                     mas[i] = default(T);
//                 }
//             }
//             else
//                 throw new IndexOutOfRangeException();
//         }
//
//
//         public static bool operator ==(NDimensionArray<T> a, NDimensionArray<T> b) =>
//             Comparisons(a, b) == 0 ? true : false;
//
//         public static bool operator !=(NDimensionArray<T> a, NDimensionArray<T> b) =>
//             Comparisons(a, b) != 0 ? true : false;
//
//
//         public static bool operator >(NDimensionArray<T> a, NDimensionArray<T> b) =>
//             Comparisons(a, b) == 1 ? true : false;
//
//         public static bool operator <(NDimensionArray<T> a, NDimensionArray<T> b) =>
//             Comparisons(a, b) == -1 ? true : false;
//
//
//         public virtual T this[params int[] ind]
//         {
//             get
//             {
//                 bool flag = flagCreating(ind);
//                 if (flag) throw new IndexOutOfRangeException();
//                 int y = ConvertLinearIndex(ind);
//                 return mas[y];
//             }
//
//             set
//             {
//                 bool flag = flagCreating(ind);
//                 if (flag) throw new IndexOutOfRangeException();
//                 int y = ConvertLinearIndex(ind);
//                 mas[y] = value;
//             }
//         }
//
//
//         IEnumerator IEnumerable.GetEnumerator()
//         {
//             return (IEnumerator) GetEnumerator();
//         }
//
//         IEnumerator<T> IEnumerable<T>.GetEnumerator()
//         {
//             return (IEnumerator<T>) GetEnumerator();
//         }
//
//         // public ElementEnum<T> GetEnumerator()
//         // {
//         //     return new ElementEnum<T>(mas);
//         // }
//
//
//         public bool Equals(NDimensionArray<T> other)
//         {
//             return this == other;
//         }
//
//
//         public override bool Equals(object obj)
//         {
//             if (obj is NDimensionArray<T> a)
//             {
//                 return Equals(a);
//             }
//
//             return false;
//         }
//
//         /// <summary>
//         /// Генерується число яке буде використовуватись як хеш даного для перного екземпляра класу
//         /// </summary>
//         /// <returns>хеш</returns>
//         public override int GetHashCode()
//         {
//             return mas.Length * dimension.Length * (dimension[0] + sizeDimension[sizeDimension.Length - 1]);
//         }
//     }
// }