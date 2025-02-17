FROM ubuntu:latest

# Enable 32-bit architecture
RUN dpkg --add-architecture i386 \
    && apt update \
    && apt install -y libc6:i386 libstdc++6:i386 \
    && apt install -y gcc-multilib g++-multilib \
    && apt install -y net-tools vim openssh-server \
    && apt clean \
    && rm -rf /var/lib/apt/lists/* \
    && echo "root:Docker!" | chpasswd

RUN mkdir -p /opt/bin
RUN mkdir -p /opt/logs
RUN mkdir -p /opt/media
RUN mkdir -p /opt/psdata

COPY scripts/sshd_config.ini /etc/ssh/sshd_config
COPY scripts/ssh_setup.sh /opt/ssh_setup.sh
COPY scripts/start.sh /opt/start.sh

COPY bin/* /opt/bin/
COPY media/* /opt/media/
COPY psdata/* /opt/psdata/

RUN /opt/ssh_setup.sh

# Set working directory
WORKDIR /app

# Default command
CMD ["/opt/start.sh"]
