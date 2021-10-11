using System;
using System.Collections.Generic;

public class Name : IComparable<Name>, IEquatable<Name>, IComparable
{
    private string First { get; set; }
    private string Middle { get; set; }
    private string Last { get; set; }
    
    // IComparable<T>
    public int CompareTo(Name other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(other, null))
        {
            return 1;
        }

        var rVal = Comparer<string>.Default.Compare(First, other.First);
        if (rVal != 0)
        {
            return rVal;
        }
        
        rVal = Comparer<string>.Default.Compare(Last, other.Last);
        return rVal != 0 ? rVal : Comparer<string>.Default.Compare(Middle, other.Middle);
    }

    public bool Equals(Name other)
    {
        if (ReferenceEquals(this,other))
        {
            return true;
        }
        if (ReferenceEquals(other,null))
        {
            return false;
        }

        return First == other.First && Middle == other.Middle && Last == other.Last;
    }

    public static bool CheckEquality(object firstObject, object secondObject)
    {
        if (firstObject == null)
        {
            return secondObject == null;
        }

        return firstObject.Equals(secondObject);
    }
    

    public override bool Equals(object obj)
    {
        return obj.GetType() == typeof(Name) ? this.Equals(obj as Name) : false;
    }

    int IComparable.CompareTo(object firstObject)
    {
        if (firstObject.GetType() != typeof(Name))
        {
            throw new ArgumentException("This object is not a Name object");
        }

        return this.CompareTo(firstObject as Name);
    }
}