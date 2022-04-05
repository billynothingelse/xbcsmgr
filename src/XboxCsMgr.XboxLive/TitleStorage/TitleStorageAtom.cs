using System;
using System.Collections.Generic;
using System.Text;

namespace XboxCsMgr.XboxLive.TitleStorage
{
    public class TitleStorageAtom
    {
        public string[] BlockIds { get; set; }

        public int Size { get; set; }

        public string Compression { get; set; }
    }

    public class TitleStorageBlobAtom
    {
        public string Name { get; set; }

        public string Atom { get; set; }

        public TitleStorageBlobAtom(string name, string atom)
        {
            Name = name;
            Atom = atom;
        }
    }

    public class TitleStorageBlobAtomUpdate
    {
        public IList<TitleStorageBlobAtom> Atoms { get; set; }

        public TitleStorageBlobAtomUpdate(IList<TitleStorageBlobAtom> atoms)
        {
            Atoms = atoms;
        }
    }
}
