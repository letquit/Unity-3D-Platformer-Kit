using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Platformer
{
    public interface IEntityFactory<T> where T : Entity
    {
        T Create(Transform spawnPoint);
    }
}