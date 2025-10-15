﻿using Content.Server._Stalker.Weight;
using Content.Server._Stalker.Weight.Modifier;
using STWeightMaximumModifierComponent = Content.Server._Stalker.Weight.Modifier.STWeightMaximumModifierComponent;

namespace Content.Server._Stalker.Characteristics.Modifiers.Weight;

public sealed class CharacteristicModifierWeightMaximumSystem : BaseCharacteristicFloatModifierSystem<CharacteristicModifierWeightMaximumComponent, STWeightMaximumModifierComponent, STWeightMaximumModifierSystem>;
