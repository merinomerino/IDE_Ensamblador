using System;
public class PlaceHolder
{
    public int index { get; }
    public string name { get; }
    public TableID tableId { get; }
    public PlaceHolder(int index, string name, TableID tableId)
    {
        this.index = index;
        this.name = name;
        this.tableId = tableId;
    }
    public override string ToString()
    {
        return name;
    }
}
public class BasicPlaceHolder : ITokenType
{
    private readonly PlaceHolder placeHolder;
    public PlaceHolder PlaceHolder { get { return placeHolder; } }
    public BasicPlaceHolder(PlaceHolder placeHolder)
    {
        this.placeHolder = placeHolder;
    }
    public T Match<T>(
        Func<string, T> fixedString,
        Func<PlaceHolder, T> basicPlaceHolder,
        Func<PlaceHolder, T> asmPlaceHolder
    )
    {
        return basicPlaceHolder(placeHolder);
    }
    public override string ToString()
    {
        return placeHolder.ToString();
    }
}
public class AsmPlaceHolder : ITokenType
{
    private readonly PlaceHolder placeHolder;
    public PlaceHolder PlaceHolder { get { return placeHolder; } }
    public AsmPlaceHolder(PlaceHolder placeHolder)
    {
        this.placeHolder = placeHolder;
    }
    public T Match<T>(
        Func<string, T> fixedString,
        Func<PlaceHolder, T> basicPlaceHolder,
        Func<PlaceHolder, T> asmPlaceHolder
    )
    {
        return asmPlaceHolder(placeHolder);
    }
    public override string ToString()
    {
        return placeHolder.ToString();
    }
}
public class FixedString : ITokenType
{
    private readonly string str;
    public string Str { get { return str; } }
    public FixedString(string str)
    {
        this.str = str;
    }
    public T Match<T>(
        Func<string, T> fixedString,
        Func<PlaceHolder, T> basicPlaceHolder,
        Func<PlaceHolder, T> asmPlaceHolder)
    {
        return fixedString(str);
    }
    public override string ToString()
    {
        return Str;
    }
}
public interface ITokenType
{
     T Match<T>(
        Func<string, T> fixedString,
        Func<PlaceHolder, T> basicPlaceHolder,
        Func<PlaceHolder, T> asmPlaceHolder
    );
}
// --------------