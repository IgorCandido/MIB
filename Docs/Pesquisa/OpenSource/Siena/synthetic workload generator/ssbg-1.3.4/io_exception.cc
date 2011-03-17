// -*- C++ -*-
//
//  This file is part of Siena, a wide-area event notification system.
//  See http://www.cs.colorado.edu/serl/siena/
//
//  Authors: See the file AUTHORS for full details. 
//
//  Copyright (C) 2002-2003 University of Colorado
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,
//  USA, or send email to serl@cs.colorado.edu.
//
// $Id: io_exception.cc,v 1.1 2004/01/26 18:01:58 carzanig Exp $
//
#include "siena/ssbg.h"

using namespace ssbg;
using namespace std;

io_exception :: io_exception( const string& arg ) throw ()
  : m_arg( arg )
{
}

io_exception :: ~io_exception() throw ()
{
}

const char* io_exception :: what() const throw()
{
  return m_arg.c_str();
}
