Security Event Trigger Instructions
-------------------------------------
This is the same content as the Instructions file.


Overview
-------------------------------------
As of 21.5, ConnectWise Control can monitor specific security related events and perform actions based upon user-defined filters.  There are concrete plans to implement this functionality into the Web Application (similar to Session Event Triggers), but in the meantime it must be manually done.  Currently available security events include LoginAttempt, LogoutAttempt, ChangePasswordAttempt, and ResetPasswordAttempt.  

Also contained within this repo is the "SecurityEvent Enums.txt" file which contains all available objects and properties upon which they can be filtered.  In general, Security Event Triggers behave almost identical to Session Event Triggers and the SessionEventTrigger.xml file (C:\Program Files (x86)\ScreenConnect\App_Data\SessionEventTrigger.xml by default) can be used as a reference.

Steps to create and implement a basic Security Event Trigger to monitor for authentication attempts where the incorrect password is used and send an email when it occurs:


1.  Create a file called SecurityEventTrigger.xml
2.  Copy and paste the below contents into the file, be sure to update the EMAIL_ADDRESS:

<?xml version="1.0"?>
<SecurityEventTriggers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<SecurityEventTrigger Name="Incorrect Password Authentication Attempt" IsDisabled="false" EventFilter="Event.EventType = 'LoginAttempt' AND Event.OperationResult = 'PasswordInvalid'">
   		<Actions>
			<TriggerAction xsi:type="SmtpTriggerAction" From="" To="EMAIL_ADDRESS" Subject="An invalid password authentication attempt has just occurred">
				<Body>{*:j}</Body>
			</TriggerAction>
		</Actions>
	</SecurityEventTrigger>
</SecurityEventTriggers>

3.  Stop all Control server services including the Web Server, Relay, Security Manager, and Session Manager
4.  Place the SecurityEventTrigger.xml file into the App_Data directory within the Control server installation(C:\Program Files (x86)\ScreenConnect\App_Data by default)
5.  Start all 4 Control server services.


Explanation
-------------------------------------
Looking at the above XML sample we see that the file is just a collection of SecurityEventTriggers.  Each SecurityEventTrigger can contain multiple TriggerAction objects which can be emails or web requests (SmtpTriggerAction, HttpTriggerAction respectively).

If we step into the defined SecurityEventTrigger, we see it has a name, it is NOT disabled, and it has an EventFilter which specifies the Security Events for which we want it to execute an action.  Looking at the EventFilter, we see it's looking for LoginAttempts where the result is "PasswordInvalid".  

Now we can look at the Actions section, which contains a single TriggerAction of type SmtpTriggerAction.  These properties are all self explanatory and correspond to a typical email message.  The Body field, however, contains a special Control-defined object.  This example tells the email body to contain all relevant objects and their properties formatted as json.  Using {*:x} will output a XML hierarchy and you can use "u" to create a URL-encoded string, such as {Event:u}.

An example of a HttpTriggerAction is:
<TriggerAction xsi:type="HttpTriggerAction" Uri="https://my.domain.com/AccessibleEndpoint" HttpMethod="POST" ContentType="application/json">
	<Body>{*:j}</Body>
</TriggerAction>

If necessary, you can add additional Http Headers with the property "ExtraHeaders".
