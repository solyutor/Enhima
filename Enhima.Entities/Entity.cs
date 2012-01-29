using System;

namespace Enhima
{
    /// <summary>
    /// Base class for all entities in domain. Simplified version of AbstractEntity class from uNhAddins project http://code.google.com/p/unhaddins/source/browse/uNhAddIns/uNhAddIns.Entities/AbstractEntity.cs
    /// </summary>
    [Serializable]
    public abstract class Entity
	{
		private int? _requestedHashCode;

		public virtual long Id { get; protected set; }

        protected bool IsTransient()
		{
			return Equals(Id, default(long));
		}

        /// <summary>
        /// Compare equality trough Id
        /// </summary>
        /// <param name="other">Entity to compare.</param>
        /// <returns>true is are equals</returns>
        /// <remarks>
        /// Two entities are equals if they are of the same hierarcy tree/sub-tree
        /// and has same id.
        /// </remarks>
		public override bool Equals(object obj)
		{
			var that = obj as Entity;
		    if (null == that || !GetType().IsInstanceOfType(that))
		    {
		        return false;
		    }
		    if (ReferenceEquals(this, that))
		    {
		        return true;
		    }

		    bool otherIsTransient = that.IsTransient();
		    bool thisIsTransient = IsTransient();
		    if (otherIsTransient && thisIsTransient)
		    {
		        return ReferenceEquals(that, this);
		    }

		    return that.Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			if (!_requestedHashCode.HasValue)
			{
				_requestedHashCode = IsTransient() ? base.GetHashCode() : Id.GetHashCode();
			}
			return _requestedHashCode.Value;
		}
	}
}