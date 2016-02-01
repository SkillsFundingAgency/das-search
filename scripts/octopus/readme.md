#Scripts for Octopus


## trigger-vsts-build.ps1
Starting a VSTS build
Set **Variable** in Octopus
 - VSTSUserName
 - VSTSToken
 - BuildConfigurationId, What build definition to queue

## send-udp.ps1
Sending a message to udp endpoint
Set **Variable** in Octopus
 - LogstashPort, open port for logstash
 - ElasticServerIp, server that logstrash is installed on
