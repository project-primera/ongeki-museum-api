FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install additional tools
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends \
    git \
    curl \
    ca-certificates \
    sudo \
    && apt-get clean -y && rm -rf /var/lib/apt/lists/*

# Set timezone
ENV TZ=Asia/Tokyo
RUN ln -snf /usr/share/zoneinfo/"$TZ" /etc/localtime && echo "$TZ" > /etc/timezone

# Create vscode user
ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=$USER_UID

RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

# Add dotnet tools to PATH
ENV PATH="${PATH}:/root/.dotnet/tools:/home/${USERNAME}/.dotnet/tools"

# Set working directory
WORKDIR /workspace
