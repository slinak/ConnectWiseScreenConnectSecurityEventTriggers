using System;
using ScreenConnect;

public class SecurityEventTriggerAccessor : IDynamicSecurityEventTrigger
{
	public Proc GetDeferredActionIfApplicable(SecurityEventTriggerEvent securityEventTriggerEvent)
	{
		if (securityEventTriggerEvent.SecurityEvent.EventType == SecurityEventType.LoginAttempt) {
				return (Proc)delegate
				{
					//return proc of action to be taken here
				};
		}
		
		return null;
	}
}