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
// $Id: ssbg_main.cc,v 1.2 2008-04-01 16:04:28 carzanig Exp $
//
#include "ssbconf.h"

#include <sys/time.h>
#include <math.h>

#include <cstdlib>
#include <iostream>
#include <fstream>
#include <algorithm>
#include <stdexcept>
#include <set>
#include <string>
#include <sstream>

#include "siena/ssbg.h"

using namespace ssbg;
using namespace std;

void print_usage(const char *name) 
{
  cerr   << "Siena Synthetic Benchmark Generator" 
	 << "(" << PACKAGE_NAME << " v." << PACKAGE_VERSION << ")" << endl
	 << "Copyright (C) 2002-2005 University of Colorado\n\
This program comes with ABSOLUTELY NO WARRANTY.\n\
This is free software, and you are welcome to redistribute it\n\
under certain conditions. See the file COPYING for details.\n\
\nusage: " << name << " [options...]\n\
options:\n\
\t -I <num>                       total number of interfaces\n\
\t -N <num>                       total number of notifications generated\n\
\t -seed <num>                    random seed\n\
\t -rt|--reuse-types y|n          reuse the initial type associated with a\n\
                                  particular attribute or constraint name.\n\
                                  default is y\n\
\n\
\t -fl|--filters-min <num>        number of filters per interface\n\
\t -fh|--filters-max <num>        number of filters per interface\n\
\t -cl|--constr-min <num>         min number of constraints per filter\n\
\t -ch|--constr-max <num>         max number of constraints per filter\n\
\t -al|--attr-min <num>           min number of attributes per notification\n\
\t -ah|--attr-max <num>           max number of attributes per notification\n\
\n\
\t -da|--dist-attributes <file>   distribution of attribute names\n\
                                  default = attr_names.dist\n\
\t -dc|--dist-constraints <file>  distribution of constraint names\n\
                                  default = constr_names.dist\n\
\t -dt|--dist-types <file>        distribution of attribute types\n\
                                  default = types.dist\n\
\t -dbo|--dist-bool-operators <file>    distribution of bool operators\n\
                                  default = bool_operators.dist\n\
\t -ddo|--dist-double-operators <file>    distribution of double operators\n\
                                  default = double_operators.dist\n\
\t -dio|--dist-int-operators <file>    distribution of int operators\n\
                                  default = int_operators.dist\n\
\t -dso|--dist-string-operators <file>    distribution of string operators\n\
                                  default = string_operators.dist\n\
\t -db|--dist-bool-values <file>  distribution of boolean values\n\
                                  default = bool_values.dist\n\
\t -dd|--dist-double-values <file>  distribution of double values\n\
                                  default = double_values.dist\n\
\t -di|--dist-int-values <file>   distribution of integer values\n\
                                  default = int_values.dist\n\
\t -ds|--dist-str-values <file>   distribution of string values\n\
                                  default = string_values.dist\n\
" << endl;
  exit(1);
}

//
// generator parameters
//
unsigned int		I			= 100;
unsigned int            N                       = 100;

//
// we generate an innitial number of filters before we generate any
// notification.  init_filters_count defines this number.
//
// int			init_filters_count	= 300;

//
// upper bound for active filters.  Filters are added only when
// filters_count < max_filters
//
// int			max_filters		= 2000;

//
// lower bound for active filters.  Filters are removed only when
// filters_count > min_filters
//
// int			min_filters		= 1000;

int			attr_min		= 1;
int			attr_max		= 10;
int			constr_min		= 1;
int			constr_max		= 5;
int			filters_min		= 1;
int			filters_max		= 20;

const char *		types_dist_f		= "types.dist";
const char *		op_bool_dist_f		= "bool_operators.dist";
const char *		op_double_dist_f	= "double_operators.dist";
const char *		op_int_dist_f		= "int_operators.dist";
const char *		op_string_dist_f	= "string_operators.dist";
const char *		attr_dist_f		= "attr_names.dist";
const char *		constr_dist_f		= "constr_names.dist";
const char *		bool_dist_f		= "bool_values.dist";
const char *		double_dist_f		= "double_values.dist";
const char *		str_dist_f		= "string_values.dist";
const char *		int_dist_f		= "int_values.dist";

long			seed			= 0;
bool			init_seed		= false;

bool			reuse_types             = true;


void parse_cmd_line(int argc, char *argv[]) {
  for (int i=1; i< argc; ++i) { 
    if (strcmp(argv[i], "-N")==0) {
      if (++i < argc) {
	N = atoi(argv[i]);
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-I")==0) {
      if (++i < argc) {
	I = atoi(argv[i]);
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-seed")==0) {
      if (++i < argc) {
	seed = atoi(argv[i]);
	init_seed = true;
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-rt")==0
	       || strcmp( argv[i], "--reuse-types" ) == 0) {
      if (++i < argc) {
	if( argv[i][0] == 'y' )
	  {
	    reuse_types = true;
	  }
	else
	  {
	    reuse_types = false;
	  }
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-fl")==0 
	       || strcmp(argv[i], "--filters-min")==0) {
      if (++i < argc) {
	filters_min = atoi(argv[i]);
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-fh")==0 
	       || strcmp(argv[i], "--filters-max")==0) {
      if (++i < argc) {
	filters_max = atoi(argv[i] );
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-cl")==0 
	       || strcmp(argv[i], "--constr-min")==0) {
      if (++i < argc) {
	constr_min = atoi( argv[i] );
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-ch")==0
	       || strcmp(argv[i], "--constr-max")==0) {
      if (++i < argc) {
	constr_max = atoi(argv[i]);
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-al")==0
	       || strcmp(argv[i], "--attr-min")==0) {
      if (++i < argc) {
	attr_min = atoi( argv[i] );
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-ah")==0
	       || strcmp(argv[i], "--attr-max")==0) {
      if (++i < argc) {
	attr_max = atoi( argv[i] );
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-da")==0
	       || strcmp(argv[i], "--dist-attributes")==0) {
      if (++i < argc) {
	attr_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-dc")==0
	       || strcmp(argv[i], "--dist-constraints")==0) {
      if (++i < argc) {
	constr_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-dbo")==0
	       || strcmp(argv[i], "--dist-bool-operators")==0) {
      if (++i < argc) {
	op_bool_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-ddo")==0
	       || strcmp(argv[i], "--dist-double-operators")==0) {
      if (++i < argc) {
	op_double_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-dio")==0
	       || strcmp(argv[i], "--dist-int-operators")==0) {
      if (++i < argc) {
	op_int_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-dso") == 0
	       || strcmp(argv[i], "--dist-string-operators") == 0)  {
      if (++i < argc) {
	op_string_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-dt")==0
	       || strcmp(argv[i], "--dist-types")==0) {
      if (++i < argc) {
	types_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-db")==0
	       || strcmp(argv[i], "--dist-bool-values")==0) {
      if (++i < argc) {
	bool_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-dd")==0
	       || strcmp(argv[i], "--dist-double-values")==0) {
      if (++i < argc) {
	double_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-di")==0
	       || strcmp(argv[i], "--dist-int-values")==0) {
      if (++i < argc) {
	int_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else if (strcmp(argv[i], "-ds")==0
	       || strcmp(argv[i], "--dist-str-values")==0) {
      if (++i < argc) {
	str_dist_f = argv[i];
      } else {
	print_usage(argv[0]);
      }
    } else {
      print_usage(argv[0]);
    }
  }
}

// specific version for strings to get the '"' characters.
ostream & output_attr(ostream & out, 
		      const string & x, 
		      const string & o, 
		      const string & v ) {
  //
  // simplistic version
  //
  return out << x << ' ' << o << ' ' << '"' << v << '"';
}

// general version works for int, bool, double (for now)
template<class T> ostream & output_attr(ostream & out, 
		      const string & x, 
		      const string & o, 
		      T v ) {
  //
  // simplistic version
  //
  return out << x << ' ' << o << ' ' << v;
}

void output_notif( SSBG& ssbg, ostream & out )
{
  notification n;
  ssbg.new_notification( n );
  
  notification::const_iterator vi;
  for( vi = n.begin(); vi != n.end(); ++vi )
    {
      out << ' ';
      char t = (*vi).type;
      switch(t) {
      case 's': output_attr(out, (*vi).name, "=", (*vi).string_val ); break;
      case 'd': output_attr(out, (*vi).name, "=", (*vi).double_val ); break;
      case 'i': output_attr(out, (*vi).name, "=", (*vi).int_val ); break;
      case 'b': output_attr(out, (*vi).name, "=", (*vi).bool_val ); break;
      default:  throw logic_error( "gen_notif: unrecognized type code" );
      }
    }
}

void output_filter( const filter& f, ostream & out) {
  filter::const_iterator vi;
  for(vi = f.begin(); vi != f.end(); ++vi) {
    if (vi != f.begin()) cout << ", ";
    char t = (*vi).type;
    switch(t) {
    case 's':
      output_attr( out, (*vi).name, (*vi).op, (*vi).string_val );
      break;
    case 'd':
      output_attr( out, (*vi).name, (*vi).op, (*vi).double_val );
      break;
    case 'i':
      output_attr( out, (*vi).name, (*vi).op, (*vi).int_val );
      break;
    case 'b':
      output_attr( out, (*vi).name, (*vi).op, (*vi).bool_val );
      break;
    default:  throw logic_error( "output_filter: unrecognized type code" );
        }
  }
}

int main(int argc, char *argv[]) 
{
  //
  // set parameters from command-line options
  //
  parse_cmd_line( argc, argv);

  //
  // initialize random seed
  //
  if (!init_seed) {
    struct timeval tv;
    gettimeofday(&tv, NULL);
    seed = tv.tv_usec;
  }
  srand(seed);

  SSBG ssbg( attr_min, attr_max,
	     constr_min, constr_max,
	     filters_min, filters_max, reuse_types );

  //
  // read distribution files
  //
  try
  {
    ssbg.load_distributions( bool_dist_f,
			     double_dist_f,
			     int_dist_f,
			     str_dist_f,
			     types_dist_f,
			     op_bool_dist_f,
			     op_double_dist_f,
			     op_int_dist_f,
			     op_string_dist_f,
			     attr_dist_f,
			     constr_dist_f );
  }
  catch( io_exception ioe )
    {
      cerr << "error: " << ioe.what() << endl;
      return 1;
    }

  //
  // okay, here I need to compute the number of filters per
  // interface, fpi.  I could apply some sophisticated distribution,
  // or better yet, I could supply a distribution file for
  // interfaces.  However for now, I'll just use a Poisson
  // distribution, assuring a mean F / I for fpi, so that for
  // reasonably high numbers of 
  //
  predicate p;

  // before doing any output, set some formatting flags:
  cout << showpoint;
  cout << boolalpha;
  for( unsigned int i = 0; i < I; ++i )
    {
      ssbg.new_predicate( p );
      predicate::const_iterator j;
      for( j = p.begin(); j != p.end(); ++j )
	{
	  if( j == p.begin() )
	    {
	      cout << "ifconfig " << i << ' ';
	      output_filter( (*j), cout );
	    }
	  else
	    {
	      cout << endl << " | ";
	      output_filter( (*j), cout );
	  }
	}
      cout << endl;
    }
  
  for( unsigned int i = 0; i < N; ++i)
    {
      cout << "select ";
      output_notif( ssbg, cout );
      cout << endl;
    }
  
  return 0;
}
