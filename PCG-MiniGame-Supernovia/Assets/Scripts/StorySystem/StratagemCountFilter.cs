using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemCountFilter : CountFilter
{
    private CharacterCard hookedCharacter;
    public StratagemCountFilter(CharacterCard hookedCharacter) {
        this.hookedCharacter = hookedCharacter;
    }

    public override int Filt() {
        return 2;
    }
}
