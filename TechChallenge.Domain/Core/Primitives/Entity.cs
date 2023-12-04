using System;

namespace TechChallenge.Domain.Core.Primitives
{
    public abstract class Entity : IEquatable<Entity>        
    {
        #region Properties

        public int Id { get; protected set; }

        #endregion

        #region Constructors

        protected Entity()
        { }

        public Entity(int idEntity) : this()
        {
            Id = idEntity;
        }

        #endregion

        #region IEquatable Members

        public bool Equals(Entity other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || Id.Equals(other.Id);
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            if (obj is not Entity compareTo) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
            => (GetType().GetHashCode() * 907) + Id.GetHashCode();

        #endregion

        #region Operators

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
            => !(a == b);

        #endregion
    }
}
