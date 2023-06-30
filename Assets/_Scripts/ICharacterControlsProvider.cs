using System;

internal interface ICharacterControlsProvider
{
    event Action Jump;
    event Action Shoot;
}