using UnityEngine;
using System.Collections.Generic;

public class PackSupplier : Singleton<PackSupplier>
{
    [field: SerializeField] public List<PuzzlePack> Packs { get; private set; }
}
