#ifndef CELL_REGISTER_SERVICES_H
#define CELL_REGISTER_SERVICES_H

class CAsioServer4C;
class CAsioServer4S;
void RegisterServices4Clt(CAsioServer4C& rSvr4C);
void RegisterServices4Svr(CAsioServer4S& rSvr4S);

#endif  // CELL_REGISTER_SERVICES_H
