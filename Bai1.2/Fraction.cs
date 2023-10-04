using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class Fraction : IComparable<Fraction>
{
    protected int numerator;
    public int Numerator
    {
        get { return numerator; }
        set { numerator = value; }
    }
    protected int denominator;
    public int Denominator
    {
        get { return denominator; }
        set { denominator = value; }
    }

    public Fraction()
    {
        Numerator = 0;
        Denominator = 1;
    }
    public Fraction(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public Fraction(int numerator)
    {
        Numerator = numerator;
        Denominator = 1;
    }

    public static Fraction operator +(Fraction lf, Fraction rf)
    {
        Fraction temp = new Fraction();
        temp.numerator = lf.numerator * rf.denominator + lf.denominator * rf.numerator;
        temp.denominator = lf.denominator * rf.denominator;
        return temp;
    }

    public static Fraction operator -(Fraction lf, Fraction rf)
    {
        Fraction temp = new Fraction();
        temp.numerator = lf.numerator * rf.denominator - lf.denominator * rf.numerator;
        temp.denominator = lf.denominator * rf.denominator;
        return temp;
    }

    public static Fraction operator *(Fraction lf, Fraction rf)
    {
        Fraction temp = new Fraction();
        temp.numerator = lf.numerator * rf.numerator;
        temp.denominator = lf.denominator * rf.denominator;
        return temp;
    }

    public static Fraction operator /(Fraction lf, Fraction rf)
    {
        Fraction temp = new Fraction();
        temp.numerator = lf.numerator * rf.denominator;
        temp.denominator = lf.denominator * rf.numerator;
        return temp;
    }

    public static bool operator ==(Fraction lf, Fraction rf)
    {
        double a = (double)lf.numerator / lf.denominator;
        double b = (double)rf.numerator / rf.denominator;
        if (a == b)
            return true;
        return false;
    }

    public static bool operator !=(Fraction lf, Fraction rf)
    {
        double a = (double)lf.numerator / lf.denominator;
        double b = (double)rf.numerator / rf.denominator;
        if (a != b)
            return true;
        return false;
    }

    public static bool operator >(Fraction lf, Fraction rf)
    {
        double a = (double)lf.numerator / lf.denominator;
        double b = (double)rf.numerator / rf.denominator;
        if (a > b)
            return true;
        return false;
    }

    public static bool operator <(Fraction lf, Fraction rf)
    {
        double a = (double)lf.numerator / lf.denominator;
        double b = (double)rf.numerator / rf.denominator;
        if (a < b)
            return true;
        return false;
    }

    public static implicit operator Fraction(int x)
    {
        return new Fraction(x);
    }

    public static explicit operator double(Fraction x)
    {
        return (double)x.numerator / x.denominator;
    }

    public int CompareTo(Fraction f)
    {
        if (this > f)
            return 1;
        else if (this < f)
            return -1;
        return 0;
    }
}
