namespace Archimedes.Maps
{
    /// <summary>
    /// The implementing class has a LocationInfo Property
    /// </summary>
    public interface IWorldLocationMember
    {
        WorldLocation LocationInfo {get; }

        bool IsPositionResolved { get; }
    }
}
