using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DrunkenDwarves;

public class PlayerDashTurret : DashObject
{
    [SerializeField] private float lifeDuration;
    private SpellcastingController controller;
    private CharacterRotator rotator;
    private PlayerInput input;
    private InputAction fireLeftAction;
    private InputAction fireRightAction;
    private Timer timer;

    private void Awake()
    {
        controller = GetComponent<SpellcastingController>();
        rotator = GetComponent<CharacterRotator>();
    }

    private void Start()
    {
        timer = new Timer(lifeDuration, () => Destroy(this.gameObject));
    }

    public override void Initialize(Transform owner)
    {
        base.Initialize(owner);

        SpellcastingController ownerController = owner.GetComponent<SpellcastingController>();

        controller.LeftSpell.OverrideBaseData(ownerController.LeftSpell.GetBaseData());
        controller.LeftSpell.OverrideSpellBoostList(ownerController.LeftSpell.SpellBoosts);

        controller.RightSpell.OverrideBaseData(ownerController.RightSpell.GetBaseData());
        controller.RightSpell.OverrideSpellBoostList(ownerController.RightSpell.SpellBoosts);

        controller.OverrideSpellTagLists(ownerController.SpellIgnoreTagList, ownerController.SpellBounceTagList);

        input = GetComponent<PlayerInput>();

        fireLeftAction = input.currentActionMap.FindAction("FireLeft");
        fireRightAction = input.currentActionMap.FindAction("FireRight");
    }

    private void Update()
    {
        timer.Update(Time.deltaTime);

        rotator.LookAt(PlayerInputConstants.reticleTransform.position);

        if (fireLeftAction.ReadValue<float>() > 0.5f)
            controller.ProcessSpell(SpellType.Left);
        if (fireRightAction.ReadValue<float>() > 0.5f)
            controller.ProcessSpell(SpellType.Right);
    }
}
