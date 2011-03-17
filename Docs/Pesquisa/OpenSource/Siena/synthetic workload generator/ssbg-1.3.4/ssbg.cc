// -*- C++ -*-
//
//  This file is part of Siena, a wide-area event notification system.
//  See http://www.cs.colorado.edu/serl/siena/
//
//  Authors:See the file AUTHORS for full details. 
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
// $Id: ssbg.cc,v 1.1 2004/01/26 18:01:58 carzanig Exp $
//
#include <cmath>
#include <fstream>
#include <set>
#include <stdexcept>
#include "siena/ssbg.h"

using namespace ssbg;

attribute :: attribute()
  : name( "" ), type( BOOL ), bool_val( false )
{
}

attribute :: attribute( const string& p_name, bool p_val )
  : name( p_name ), type( BOOL ), bool_val( p_val )
{
}

attribute :: attribute( const string& p_name, double p_val )
  : name( p_name ), type( DOUBLE ), double_val( p_val )
{
}

attribute :: attribute( const string& p_name, int p_val )
  : name( p_name ), type( INT ), int_val( p_val )
{
}

attribute :: attribute( const string& p_name, const string& p_val )
  : name( p_name ), type( STRING ), int_val( 0 ), string_val( p_val )
{
}

attribute :: attribute( const attribute& a )
  : name( a.name ), type( a.type ), int_val( 0 ), string_val( "" )
{
  switch( type )
    {
    case BOOL:
      bool_val = a.bool_val;
      break;
    case DOUBLE:
      double_val = a.double_val;
      break;
    case INT:
      int_val = a.int_val;
      break;
    case STRING:
      string_val = a.string_val;
      break;
    default:
      throw logic_error( "unexpected attribute type" );
    }
}

constraint :: constraint()
  : name( "" ), type( BOOL ), op( "" ), bool_val( false )
{
}

constraint :: constraint( const string& p_name, const string& p_op, bool p_val )
  : name( p_name ), type( BOOL ), op( p_op ), bool_val( p_val )
{
}

constraint :: constraint( const string& p_name, const string& p_op, double p_val )
  : name( p_name ), type( DOUBLE ), op( p_op ), double_val( p_val )
{
}

constraint :: constraint( const string& p_name, const string& p_op, int p_val )
  : name( p_name ), type( INT ), op( p_op ), int_val( p_val )
{
}

constraint :: constraint( const string& p_name, const string& p_op, const string& p_val )
  : name( p_name ), type( STRING ), op( p_op ), int_val( 0 ), string_val( p_val )
{
}

constraint :: constraint( const constraint& a )
  : name( a.name ), type( a.type ), op( a.op ), int_val( 0 ), string_val( "" )
{
  switch( type )
    {
    case BOOL:
      bool_val = a.bool_val;
      break;
    case DOUBLE:
      double_val = a.double_val;
      break;
    case INT:
      int_val = a.int_val;
      break;
    case STRING:
      string_val = a.string_val;
      break;
    default:
      throw logic_error( "unexpected constraint type" );
    }
}

SSBG :: SSBG( int attr_min, int attr_max,
	      int constr_min, int constr_max,
	      int filters_min, int filters_max, bool reuse_types )
  : m_attr_min( attr_min ), m_attr_max( attr_max ),
    m_constr_min( constr_min ), m_constr_max( constr_max ),
    m_filters_min( filters_min ), m_filters_max( filters_max ),
    m_reuse_types( reuse_types )
{
}

int SSBG :: attr_min() const
{
  return m_attr_min;
}

int SSBG :: attr_max() const
{
  return m_attr_max;
}

int SSBG :: constr_min() const
{
  return m_constr_min;
}

int SSBG :: constr_max() const
{
  return m_constr_max;
}

template<class T> void SSBG :: load_distribution( const char* fname, wset<T>& dist )
{
  ifstream in( fname );
  if( !in )
    {
      string msg = "unable to open file: " + string( fname );
      throw io_exception( msg );
    }
  dist.read_from( in );
  in.close();
}

void SSBG :: load_distributions( const char* bool_dist_f,
				 const char* double_dist_f,
				 const char* int_dist_f,
				 const char* str_dist_f,
				 const char* types_dist_f,
				 const char* op_bool_dist_f,
				 const char* op_double_dist_f,
				 const char* op_int_dist_f,
				 const char* op_string_dist_f,
				 const char* attr_dist_f,
				 const char* constr_dist_f )
{
  load_distribution( bool_dist_f, m_bool_dist );
  load_distribution( double_dist_f, m_double_dist );
  load_distribution( int_dist_f, m_int_dist );
  load_distribution( str_dist_f, m_str_dist );
  load_distribution( types_dist_f, m_types_dist ); 
  load_distribution( op_bool_dist_f, m_op_bool_dist );
  load_distribution( op_double_dist_f, m_op_double_dist );
  load_distribution( op_int_dist_f, m_op_int_dist );
  load_distribution( op_string_dist_f, m_op_string_dist );
  load_distribution( attr_dist_f, m_attr_dist );
  load_distribution( constr_dist_f, m_constr_dist );
}

predicate* SSBG :: new_predicate()
{
  return &new_predicate( *new predicate() );
}

predicate& SSBG :: new_predicate( predicate& p )
{
  if( !p.empty() )
    {
      p.clear();
    }
  unsigned int fcount = rand_normal_range( m_filters_min, m_filters_max );
  for( unsigned int i = 0; i < fcount; ++i )
    {
      filter f;
      set<string> constrs;
      unsigned int ccount = rand_normal_range( m_constr_min, m_constr_max );
      
      while( constrs.size() < ccount )
	{
	  constrs.insert( m_constr_dist.rand_pick() );
	}

      set<string>::const_iterator vi;
      
      for(vi = constrs.begin(); vi != constrs.end(); ++vi) {
	char t = pick_type( *vi );
	switch(t) {
	case 's':
	  f.push_back( constraint( *vi,
				   m_op_string_dist.rand_pick(),
				   m_str_dist.rand_pick() ) );
	  break;
	case 'd':
	  f.push_back( constraint( *vi,
				   m_op_double_dist.rand_pick(),
				   m_double_dist.rand_pick() ) );
	  break;
	case 'i': 
	  f.push_back( constraint( *vi,
				   m_op_int_dist.rand_pick(),
				   m_int_dist.rand_pick() ) );
	  break;
	case 'b':
	  f.push_back( constraint( *vi,
				   m_op_bool_dist.rand_pick(),
				   m_bool_dist.rand_pick() ) );
	  break;
	default:
	  throw logic_error( "unrecognized type character" );
	}
      }
      p.push_back( f );
    }
  return p;
}

notification* SSBG :: new_notification()
{
  return &new_notification( *new notification() );
}

notification& SSBG :: new_notification( notification& n )
{
  if( !n.empty() )
    {
      n.clear();
    }
  set<string> attrs;

  unsigned int acount = rand_normal_range( m_attr_min, m_attr_max );

  while( attrs.size() < acount )
    {
      attrs.insert( m_attr_dist.rand_pick() );
    }

  set<string>::const_iterator vi;
  for(vi = attrs.begin(); vi != attrs.end(); ++vi) {
    char t = pick_type( *vi );
    switch(t) {
    case 's':
      n.push_back( attribute( *vi, m_str_dist.rand_pick() ) );
      break;
    case 'i':
      n.push_back( attribute( *vi, m_int_dist.rand_pick() ) );
      break;
    case 'b':
      n.push_back( attribute( *vi, m_bool_dist.rand_pick() ) );
      break;
    case 'd':
      n.push_back( attribute( *vi, m_double_dist.rand_pick() ) );
      break;
    default:
      throw logic_error( "unrecognized type character" );
    }
  }
  return n;
}

inline char SSBG :: pick_type( const string& str )
{
  if( m_reuse_types )
    {
      map<string,char>::const_iterator idx = m_type_map.find( str );
      if( idx != m_type_map.end() )
	{
	  return (*idx).second;
	}
      else
	{
	  char c = m_types_dist.rand_pick();
	  m_type_map[str] = c;
	  return c;
	}
    }
  else
    {
      return m_types_dist.rand_pick();
    }
}

int ssbg :: rand_normal(int max) {
    return (int) (1.0*max*rand()/(RAND_MAX+1.0));
}

int ssbg :: rand_normal_range(int min, int max) {
    return min + rand_normal(max - min);
}

unsigned long ssbg :: rand_poisson_delta(unsigned long mean)
{
    double rn = 1.0 - 1.0*rand()/(RAND_MAX+1.0);

    return (unsigned long)(-(1.0*mean)*log(rn));
};
