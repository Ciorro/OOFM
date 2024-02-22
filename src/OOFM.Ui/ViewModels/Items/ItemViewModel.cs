using CommunityToolkit.Mvvm.ComponentModel;

namespace OOFM.Ui.ViewModels.Items;

internal abstract partial class ItemViewModel<T>(T item) : ObservableObject, IEquatable<ItemViewModel<T>>
    where T : class
{
    protected readonly T Item = item;

    public bool Equals(ItemViewModel<T>? other)
    {
        if (other is null)
            return false;
        return Item.Equals(other.Item);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ItemViewModel<T>);
    }

    public override int GetHashCode()
    {
        return Item.GetHashCode();
    }

    public static bool operator ==(ItemViewModel<T> i1, ItemViewModel<T> i2)
    {
        if (i1 is null)
            return i2 is null;
        return i1.Equals(i2);
    }

    public static bool operator !=(ItemViewModel<T> i1, ItemViewModel<T> i2)
    {
        return !(i1 == i2);
    }

    public static explicit operator T(ItemViewModel<T> itemViewModel)
    {
        return itemViewModel.Item;
    }
}
