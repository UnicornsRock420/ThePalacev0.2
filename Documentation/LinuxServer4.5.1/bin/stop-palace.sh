#!/bin/sh

root=/opt

if [ -f $root/logs/pserver.pid ]
then
  PID=`cat $root/logs/pserver.pid`
  if kill -0 $PID 2> /dev/null
  then
    kill -TERM $PID 2> /dev/null
    sleep 3
    if kill -0 $PID 2> /dev/null
    then
      kill -KILL $PID 2> /dev/null
      sleep 3
    fi
    if kill -0 $PID 2> /dev/null
    then
      echo "ERROR: Palace Server isn't shutting down"
    else
      echo "Palace Server shut down"
    fi
  else
    echo "ERROR: Palace Server already shut down, or invalid permissions"
  fi
  rm $root/logs/pserver.pid
else
  echo "ERROR: Palace Server isn't running"
fi
