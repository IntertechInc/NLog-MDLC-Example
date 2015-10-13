# NLog-MDLC-Example
Example of using NLog Mapped Diagnostics Logical Context to add custom elements to logging output.

Adding Custom Log Properties, e.g. TrackingId (Optional)
********************************************************
If you need to add custom properties like a TrackingId to your log output you can use the MappedDiagnosticsLogicalContext (MDLC) Layout Renderer made available in 
NLog as of version 4.1.2. Previously MDLC was available in NLog.Contrib. MappedDiagnosticsLogicContext is similar to the NLog native MappedDiagnosticsContext, however it 
maintains the value of your properties across asynchronous context, where MappedDiagnosticContext is stored in thread local storage and is only available to the initializing 
thread. 

The example below describes adding a logging property called "trackingId" to your logging output

Steps to include trackingId as a log output property:
1. Add the new property to your logging config file making sure to reference the "mdlc" layout renderer as follows:

    a. Example for file logging (note use of ${mdlc:item=trackingId})

        <target xsi:type="File" name="f" fileName="${basedir}/logs/${shortdate}.log" layout="${longdate} ${uppercase:${level}} ${mdlc:item=trackingId} ${message}" />
        
    b. Example for sql database logging (note use of @trackingId parameter in both the sql and parameter list; also note you will need to add the trackingId column 
	   to your logging table

        <target xsi:type="Database" name="db">
            <dbProvider>System.Data.SqlClient</dbProvider>
            <connectionStringName>LogConnection</connectionStringName>
            <commandText>
                INSERT INTO Logging(Application, Level, Logger, Message, MachineName, UserName, Thread, Exception, TrackingId) 
                VALUES (@appName, @level, @logger, @message, @machinename, @user_name, @threadid, @log_exception, @trackingId);
            </commandText>
            <parameter name="@appName" layout="${appName}" />
            <parameter name="@level" layout="${level}" />
            <parameter name="@logger" layout="${logger}" />
            <parameter name="@message" layout="${message}" />
            <parameter name="@machinename" layout="${machinename}" />
            <parameter name="@user_name" layout="${windows-identity:domain=true}" />
            <parameter name="@threadid" layout="${threadid}" />
            <parameter name="@log_exception" layout="${exception:format=tostring}" />
            <parameter name="@trackingId" layout="${mdlc:item=trackingId}" />
        </target>

4. Finally, set the "trackingId" in code prior to any logging occuring (Note: Ideally this value would
   be known and set as early as possible)

            string trackingId = Guid.NewGuid().ToString();
            NLog.MappedDiagnosticsLogicalContext.Set("trackingId", trackingId);
            
			// From this point on "trackindId" will be logged in your logging output. No additional code is required.
