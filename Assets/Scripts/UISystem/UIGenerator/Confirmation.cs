using System;

public class Confirmation
{
    private ConfirmationModule confirmationModule;

    public static Confirmation Confirm(string message)
    {
        Confirmation confirmation = new Confirmation
        {
            confirmationModule = ConfirmationModule.Of()
                .OnConfirm(UIController.DeleteModules)
                .OnCancel(UIController.DeleteModules)
        };

        UIController.GeneratePopup("ConfirmationPopup",
            TextModule.Message(message),
            LineModule.Create(),
            confirmation.confirmationModule);
        
        return confirmation;
    }

    public Confirmation OnConfirm(Action callback)
    {
        confirmationModule.OnConfirm(modules =>
        {
            callback();
            UIController.DeleteModules(modules);
        });
        return this;
    }

    public Confirmation OnCancel(Action callback)
    {
        confirmationModule.OnCancel(modules =>
        {
            callback();
            UIController.DeleteModules(modules);
        });
        return this;
    }
}
