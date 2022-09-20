pgadmin_port = ENV["VAGRANT_PGADMIN_PORT"] || 16543
api_port = ENV["VAGRANT_API_PORT"] || 8080

Vagrant.configure("2") do |config|
    config.vm.network "public_network"
    config.vm.network "forwarded_port", guest: 8080, host: api_port
    config.vm.network "forwarded_port", guest: 16543, host: pgadmin_port
    config.vm.define "security-srv" do |server|
        server.vm.box = "ubuntu/focal64"
        server.vm.hostname = "security-srv"
        server.vm.provider "virtualbox" do |vb|
            vb.memory = "2048"
            vb.cpus = "4"
            vb.name = "security-srv"
        end
    end
end