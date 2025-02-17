#!/bin/sh

root=/opt

if [ -f $root/logs/pserver.pid ]
then
  if kill -0 `cat $root/logs/pserver.pid` 2> /dev/null
  then
    echo "That Palace Server is already running"
    exit
  fi
fi

(
 $root/bin/pserver -f $root/psdata/pserver.conf -s $root/psdata/plugin.conf &
 echo $! > $root/logs/pserver.pid
)
