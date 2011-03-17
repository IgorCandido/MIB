dnl
dnl  This file is part of Siena, a wide-area event notification system.
dnl  See http://www.cs.colorado.edu/serl/siena/
dnl
dnl  Author: See the file AUTHORS for details. 
dnl
dnl  Copyright (C) 2001-2003 University of Colorado
dnl
dnl  This program is free software; you can redistribute it and/or
dnl  modify it under the terms of the GNU General Public License
dnl  as published by the Free Software Foundation; either version 2
dnl  of the License, or (at your option) any later version.
dnl
dnl  This program is distributed in the hope that it will be useful,
dnl  but WITHOUT ANY WARRANTY; without even the implied warranty of
dnl  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
dnl  GNU General Public License for more details.
dnl
dnl  You should have received a copy of the GNU General Public License
dnl  along with this program; if not, write to the Free Software
dnl  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,
dnl  USA, or send email to serl@cs.colorado.edu.
dnl
dnl $Id: configure.ac,v 1.4 2008-04-01 16:12:48 carzanig Exp $
dnl
AC_INIT(ssbg, 1.3.4, Antonio Carzaniga <firstname.lastname@unisi.ch>)
AC_REVISION($Revision: 1.4 $)
AC_CONFIG_AUX_DIR(config)
dnl
AM_INIT_AUTOMAKE
AM_CONFIG_HEADER(ssbconf.h:ssbconf.h.in)
dnl
AC_PREFIX_DEFAULT(/opt)
#
# libtool versioning.  Follow this rule:
# 
# if (interface changed) {
#    LT_CURRENT := LT_CURRENT + 1
#    LT_REVISION := 0
#    if (backward compatible) {
#       LT_AGE := LT_AGE + 1
#    } else {
#       LT_AGE := 0
#    }
# } else {  /* no interface changes */
#    LT_REVISION := LT_REVISION + 1
# }
#
LT_RELEASE=1
LT_CURRENT=3
LT_REVISION=4
LT_AGE=0
#
# Caveat: don't try to maintain the package version in sync with the
# ======  library version at all costs.  This is a common mistake.
#
# _LDFLAGS =  -version-info $(LT_CURRENT):$(LT_REVISION):$(LT_AGE) \
#             -release $(LT_RELEASE)
#
# LT_CURRENT is the "major" library version.  
#	Change LT_CURRENT whenever you brake interface compatibility
#
# LT_REVISION is the revision number (i.e., "minor" number)
#	Change LT_CURRENT whenever you change the implementation, but
#	reset LT_VERSION=0 whenever you change LT_CURRENT
#
#
# LT_AGE indicates a compatibility range, referring to LT_CURRENT
#	e.g., 5:34:2 means also compatible with 3:*:* and 4:*:*
#
# LT_RELEASE is the label associated with the file name.  It makes sense 
#	to assign the "major" package version number to LT_RELEASE: 
#
AC_SUBST(LT_RELEASE)
AC_SUBST(LT_CURRENT)
AC_SUBST(LT_REVISION)
AC_SUBST(LT_AGE)
#
# configuration options
#
AC_ARG_ENABLE(profiling, 
  AC_HELP_STRING([--enable-profiling],
	[include profiling information (default is NO)]),
dnl this is to optionally compile with profiling
dnl I don't know too much about this, but it looks like
dnl -pg only works with static libraries, so I'm going to 
dnl disable shared libraries here.
  [ case "$enableval" in
        coverage )
	    CFLAGS_prof='-pg -fprofile-arcs -ftest-coverage'
	    CXXFLAGS_prof='-pg -fprofile-arcs -ftest-coverage'
	    LDFLAGS_prof='-pg'
	    AC_MSG_RESULT([Enabling profiling and coverage information])
	    ;;
        * )
	    CFLAGS_prof='-pg'
	    CXXFLAGS_prof='-pg'
	    LDFLAGS_prof='-pg'
	    AC_MSG_RESULT([Enabling profiling information])
	    ;;
    esac
    AC_DISABLE_SHARED ], 
  [ CFLAGS_prof=''
    CXXFLAGS_prof=''
    LDFLAGS_prof=''
    AC_ENABLE_SHARED ])
AC_SUBST(CFLAGS_prof)
AC_SUBST(CXXFLAGS_prof)
AC_SUBST(LDFLAGS_prof)
#
# Checks for programs.
AC_PROG_MAKE_SET
AC_PROG_INSTALL
#
# The libtool macro AC_PROG_LIBTOOL checks for the C preprocessor.
# Configure gets confused if we try to check for a C preprocessor
# without first checking for the C compiler
# (see http://sources.redhat.com/ml/autoconf/2001-07/msg00036.html),
# so we invoke AC_PROG_CC explicitly.
AC_PROG_CC
AC_PROG_CXX
AC_LANG(C++)
#
AC_PROG_YACC
AM_PROG_LEX
#
AC_PROG_LIBTOOL
#
# programs
#
AC_PATH_PROG(SH, sh, /bin/sh)
AC_PATH_PROG(FALSE, false, /usr/bin/false)
AC_PATH_PROG(TRUE, true, /usr/bin/true)
AC_PATH_PROG(RM, rm, $FALSE)
AC_PATH_PROG(MV, mv, $FALSE)
AC_PATH_PROG(CP, cp, $FALSE)
AC_PATH_PROG(TAR, tar, $FALSE)
AC_PATH_PROG(GZIP, gzip, $FALSE)
#
# I should probably create a better test for rpmbuild
#
AC_PATH_PROG(RPMBUILD, rpmbuild rpm, $FALSE)
#
# library, header and othere system-specific stuff
#
AC_CONFIG_FILES([Makefile])
AC_OUTPUT
