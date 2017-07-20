#Generate server protobuf cpp files to svr_common/pb/svr/.
#Generate descriptors to scripts/pb/descriptors_svr.pb.

protoc -I. --cpp_out=../svr_common/pb/svr --descriptor_set_out=../../scripts/pb/descriptors_svr.pb *.proto
