@rem Sample properties:
@rem /p:IgnoreLocalModifications=true 
@rem /verbosity:detailed

msbuild build.msbuild /p:IgnoreLocalModifications=true /fl /flp:LogFile=build_log.txt /m