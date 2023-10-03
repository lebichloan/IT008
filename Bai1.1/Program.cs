using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Point
    {
        public double x;
        public double y;
        public Point()
        {
            x = 0;
            y = 0;
        }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public void Input()
        {
            Console.Write("Nhap x: ");
            x = double.Parse(Console.ReadLine());
            Console.Write("Nhap y: ");
            y = double.Parse(Console.ReadLine());
        }
        public void Output()
        {
            Console.WriteLine($"({x},{y})");
        }
        public double Distance(Point A)
        {
            double temp = Math.Sqrt(Math.Pow(A.x - x, 2) + Math.Pow(A.y - y, 2));
            return temp;
        }
    }
    public abstract class Shape
    {
        public string ShapeName;
        public int VerticeNumber;
        public Point[] Vertice;
        public Shape()
        {
            ShapeName = "";
            VerticeNumber = 0;
            Vertice = new Point[0];
        }
        public Shape(string shapeName, int verticeNumber, Point[] vertice)
        {
            ShapeName = shapeName;
            VerticeNumber = verticeNumber;
            Vertice = vertice;
        }
        public abstract void Input();
        public abstract double Acreage();
        public abstract void Draw();
    }
    public class Circle : Shape
    {
        public double R;
        public Circle()
        {
            R = 0;
            ShapeName = "Circle";
            VerticeNumber = 1;
            Vertice = new Point[1];
            Vertice[0] = new Point();
        }
        public override void Input()
        {
            Console.WriteLine("Nhap thong so hinh tron:");
            Console.WriteLine("Nhap cac dinh");
            Console.WriteLine("Nhap tam I: ");
            Vertice[0].Input();
            Console.WriteLine("Nhap ban kinh R: ");
            R = double.Parse(Console.ReadLine());
        }
        public override double Acreage()
        {
            return R * R * 3.14;
        }
        public override void Draw()
        {
            Console.WriteLine("Tam duong tron I");
            Vertice[0].Output();
            Console.WriteLine($"Ban kinh R = {R}");
            Console.WriteLine($"Duong kinh D = {R * 2}");
        }
    }

    public class Triangle : Shape
    {
        public Triangle()
        {
            ShapeName = "Triangle";
            VerticeNumber = 3;
            Vertice = new Point[3];
            for (int i = 0; i < VerticeNumber; i++)
            {
                Vertice[i] = new Point();
            }
        }
        public override void Input()
        {
            Console.WriteLine("Nhap thong so hinh tam giac:");
            Console.WriteLine("Nhap cac dinh");
            for (int i = 0; i < VerticeNumber; i++)
            {
                Console.WriteLine($"Nhap dinh thu {i + 1}");
                Vertice[i].Input();
            }
        }
        public override double Acreage()
        {
            double a = Vertice[0].Distance(Vertice[1]);
            double b = Vertice[1].Distance(Vertice[2]);
            double c = Vertice[2].Distance(Vertice[0]);
            double p = (a + b + c) / 2;
            double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return area;
        }
        public override void Draw()
        {
            Console.WriteLine($"3 dinh tam giac: A({Vertice[0].x},{Vertice[0].y}), B({Vertice[1].x},{Vertice[1].y}), C({Vertice[2].x},{Vertice[2].y})");
            Point G = new Point();
            G.x = (Vertice[0].x + Vertice[1].x + Vertice[2].x) / 3;
            G.y = (Vertice[0].y + Vertice[1].y + Vertice[2].y) / 3;
            Console.WriteLine($"Trong tam tam giac G({G.x},{G.y})");
        }
    }
    public class Rectangle : Shape
    {
        public Rectangle()
        {
            ShapeName = "Rectangle";
            VerticeNumber = 4;
            Vertice = new Point[4];
            for (int i = 0; i < VerticeNumber; i++)
            {
                Vertice[i] = new Point();
            }
        }
        public override void Input()
        {
            Console.WriteLine("Nhap thong so hinh chu nhat:");
            Console.WriteLine("Nhap cac dinh");
            for (int i = 0; i < VerticeNumber; i++)
            {
                Console.WriteLine($"Nhap dinh thu {i + 1}");
                Vertice[i].Input();
            }
        }
        public override double Acreage()
        {
            double a = Vertice[0].Distance(Vertice[1]);
            double b = Vertice[1].Distance(Vertice[2]);
            double c = Vertice[2].Distance(Vertice[0]);
            double p = (a + b + c) / 2;
            double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return area * 2; ;
        }
        public Point Center()
        {
            Point O = new Point();
            O.x = (Vertice[0].x + Vertice[1].x + Vertice[2].x + Vertice[3].x) / 4;
            O.y = (Vertice[0].y + Vertice[1].y + Vertice[2].y + Vertice[3].y) / 4;
            return O;
        }
        public override void Draw()
        {
            Console.WriteLine($"4 dinh hinh chu nhat: A({Vertice[0].x},{Vertice[0].y}), B({Vertice[1].x},{Vertice[1].y}), C({Vertice[2].x},{Vertice[2].y}), D({Vertice[3].x},{Vertice[3].y})");
            Point O = new Point();
            O = Center();
            Console.WriteLine($"Giao diem 2 duong cheo O({O.x},{O.y}) ");
            double[] a = new double[3];
            a[0] = Vertice[0].Distance(Vertice[1]);
            a[1] = Vertice[1].Distance(Vertice[2]);
            a[2] = Vertice[2].Distance(Vertice[0]);
            Array.Sort(a);
            Console.WriteLine($"Chieu rong: {a[0]}");
            Console.WriteLine($"Chieu dai: {a[1]}");
        }
    }
    public class Square : Rectangle
    {
        public Square()
        {
            ShapeName = "Square";
            VerticeNumber = 4;
            Vertice = new Point[4];
            for (int i = 0; i < VerticeNumber; i++)
            {
                Vertice[i] = new Point();
            }
        }
        public override void Input()
        {
            Console.WriteLine("Nhap thong so hinh vuong:");
            Console.WriteLine("Nhap cac dinh");
            for (int i = 0; i < VerticeNumber; i++)
            {
                Console.WriteLine($"Nhap dinh thu {i + 1}");
                Vertice[i].Input();
            }
        }
        public override void Draw()
        {
            Console.WriteLine($"4 dinh hinh vuong: A({Vertice[0].x},{Vertice[0].y}), B({Vertice[1].x},{Vertice[1].y}), C({Vertice[2].x},{Vertice[2].y}), D({Vertice[3].x},{Vertice[3].y})");
            Point O = new Point();
            O = Center();
            Console.WriteLine($"Giao diem 2 duong cheo O({O.x},{O.y}) ");
            double[] a = new double[3];
            a[0] = Vertice[0].Distance(Vertice[1]);
            a[1] = Vertice[1].Distance(Vertice[2]);
            a[2] = Vertice[2].Distance(Vertice[0]);
            Array.Sort(a);
            Console.WriteLine($"Do dai canh: {a[0]}");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Nhap so hinh muon tao: ");
            int n = int.Parse(Console.ReadLine());
            Shape[] shape = new Shape[n];
            for (int i = 0; i < n; i++)
            {
                Random random = new Random();
                int a = random.Next(1, 5);
                switch (a)
                {
                    case 1:
                        shape[i] = new Circle();
                        break;
                    case 2:
                        shape[i] = new Triangle();
                        break;
                    case 3:
                        shape[i] = new Rectangle();
                        break;
                    case 4:
                        shape[i] = new Square();
                        break;
                }
                shape[i].Input();
            }
            for (int i = 0; i < shape.Length; i++)
            {
                Console.WriteLine();
                shape[i].Draw();
                Console.WriteLine($"Dien tich: {shape[i].Acreage()}");

            }
            Console.ReadKey();
        }
    }
}
